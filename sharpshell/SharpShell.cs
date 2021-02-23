using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using sharpshell.lib;
using sharpshell.process;
using sharpshell.mod;
using sharpshell.misc;
using sharpshell.rule;
using static sharpshell.misc.Util;

namespace sharpshell
{
    public class SharpShell
    {
        // 重要なやつ
        private string UserName;
        private string WhereAmI;
        // 設定類 (別ファイルに分けてもいいかも。うん。ワタシもそう思う。)
        private Dictionary<string, dynamic> Settings;
        private string PromptStyle;
        public List<string> Path;// パスを通すとかのパス。
        public readonly List<string> BuiltinCmd = new List<string>(){"ls", "cat", "cd", "pwd", "whoami"};// ?
        // モジュール類
        private Listener _listener;
        private Assignor _assignor;
        // private ProcessManager _process;
        // その他
        private JsonLoader.Loader _jsonLoader = new JsonLoader.Loader();

        
        public SharpShell()
        {
            UserName = WhoAmI();
            WhereAmI = $"/Users/{WhoAmI()}/";

            Init();
            
            _listener = new Listener();
            _assignor = new Assignor(WhereAmI, Path, BuiltinCmd);
            // _process = new ProcessManager();
            
            LineStreaming();
        }

        private void Init()
        {
            // 設定読み込み
            // ちなみにこのパスはテスト用
            // 本番では"~/.sharpshellrc"を参照する予定。
            LoadSettings("/Users/x0y14/dev/csharp/sharpshell/sharpshell/setting.json");
            
            // 起動時に表示されるもの(これもGenPromptと同じフォーマットにできればおもろいかも)
            if (Settings.ContainsKey("welcome-message"))
                Console.WriteLine(Settings["welcome-message"]);
        }

        
        private void LoadSettings(string path)
        {
            // 設定の読み込みを試みる
            try
            {
                Settings = _jsonLoader.LoadWithPath(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            // スタイルの設定
            // プロンプトスタイルの初期値
            PromptStyle = $"%whoami $ ";
            if (Settings.ContainsKey("style") && Settings["style"] is Dictionary<string, dynamic>)
            {
                // スタイルでプロンプトが設定されているか。
                Dictionary<string, dynamic> styleSettings = Settings["style"];
                if (styleSettings.ContainsKey("prompt") && styleSettings["prompt"] is string)
                {
                    PromptStyle = styleSettings["prompt"];
                }
            }

            // path読み込み。
            if (Settings.ContainsKey("path"))
            {
                try
                {
                    // JsonLoaderではjsonにおける配列(list)はList<object>になっているので、無理矢理List<string>へ変換
                    Path = ConvertPathList(Settings["path"]);
                }
                catch (Exception e)
                {
                    throw new Exception("list of path can not convert, object to string");
                }
            }
            
        }

        // ------------------------------------------------------------------------
        private void LineStreaming()
        {
            // sharminal側で無限ループさせるのもありかも
            while (true)
            {
                Command cmd = _listener.Listening(GenPrompt());
                Task task = _assignor.Assign(cmd);
                if (task.TaskType == TaskType.BUILTIN)
                {
                    // built-inはこっちで割り当てするようにしてるが、のちに帰る可能性高い。
                    var result = AssignBuiltin(task);
                    Console.Write(result);
                }
                else
                {
                    Console.WriteLine("this task is not supporting.");
                }
            }
        }


        public string AssignBuiltin(Task task)
        {
            // ビルトインの割り当て"ls"しかしてないし、プロセスマネジャの設定もしてないので"ls"しか動かん。
            switch (task.Command.Fn)
            {
                case "ls":
                {
                    // [0]は一時的な処理、つまり妥協。めんどくさい。直すの。(><)
                    return Ls(task.Command.Args, task.Command.Ops);
                }
                
            }
            return $"built-in not found: `{task.Command.Fn}`";
        }
        
        
        // ------------------------------------------------------------------------
        // プロンプト
        
        public string ExchangePromptFormat(string format)
        {
            switch (format)
            {
                case "%whoami":
                {
                    return WhoAmI();
                }

                case "%whereami_name":
                {
                    var whereamiRemovedLastSlash = WhereAmI.Substring(0, WhereAmI.Length - 1);
                    var lastSlash = whereamiRemovedLastSlash.LastIndexOf("/");
                    return WhereAmI.Substring(lastSlash+1, whereamiRemovedLastSlash.Length-lastSlash-1);
                }

                case "%whereami_full":
                {
                    return WhereAmI;
                }
            }

            return format;
        }

        public string GenPrompt()
        {
            var prompt = PromptStyle;
            
            List<string> supportingFormat = new List<string>() { "%whoami", "%whereami_name", "%whereami_full" };

            foreach (var format in supportingFormat)
            {
                if (prompt.Contains(format))
                {
                    prompt = prompt.Replace(format, ExchangePromptFormat(format));
                }
            }
            
            return prompt;
        }
        
        
        // ------------------------------------------------------------------------
        // CMDs
        
        // use /usr/bin/whoami
        public string WhoAmI()
        {
            var whoami = new WhoAmI();
            return whoami.GetUserName();
        }
        
        public string Ls(List<string> args, List<string> ops)
        {
            string _args;
            if (!args.Any())
            {
                _args = "";
            }
            else
            {
                _args = args[0];
            }
            
            var ls = new Ls();
            return ls.GetFileAndDirectoryNameOnlyPrint(WhereAmI, _args, ops);
        }
        
        public string Cd(string path)
        {
            var cd = new Cd();
            // WhereAmI = cd.Move(WhereAmI, path);// 時間かかる。
            WhereAmI = cd.MoveQuick(WhereAmI, path);
            return WhereAmI;
        }
        
        public string Pwd()
        {
            return WhereAmI;
        }

        public string Cat(string path)
        {
            return "";
        }
        
    }
}