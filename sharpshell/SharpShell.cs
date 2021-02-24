using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using sharpshell.builtin;
using sharpshell.process;
using sharpshell.mod;
using sharpshell.misc;
using sharpshell.rule;
using sharpshell.setting;
using sharpshell.wd;
using static sharpshell.misc.Util;

namespace sharpshell
{
    public class SharpShell
    {
        // モジュール類
        private Listener _listener;
        private Parser _parser;
        private Assignor _assignor;
        private SettingsManager _settingsManager;
        private WorkingDirectoryManager _workingDirectoryManager;
        private BuiltinManager _builtinManager;
        // private ProcessManager _process;
        // その他
        private JsonLoader.Loader _jsonLoader = new JsonLoader.Loader();

        
        public SharpShell()
        {
            _workingDirectoryManager = new WorkingDirectoryManager($"/Users/{BuiltinManager.WhoAmI()}/");
            _settingsManager = new SettingsManager("/Users/x0y14/dev/csharp/sharpshell/sharpshell/setting.json");
            _listener = new Listener();
            _parser = new Parser();
            _assignor = new Assignor(_settingsManager.GetPath());
            _builtinManager = new BuiltinManager();
            // _process = new ProcessManager();
            Init();
            LineStreaming();
        }

        private void Init()
        {
            // 設定読み込み
            // ちなみにこのパスはテスト用
            // 本番では"~/.sharpshellrc"を参照する予定。
            
            // 起動時に表示されるもの(これもGenPromptと同じフォーマットにできればおもろいかも)
            if (_settingsManager.UserSettings.ContainsKey("welcome-message"))
                Console.WriteLine(_settingsManager.UserSettings["welcome-message"]);
        }
        
        
        // ------------------------------------------------------------------------
        private void LineStreaming()
        {
            // sharminal側で無限ループさせるのもありかも
            while (true)
            {
                string userInput = _listener.Listening(GenPrompt());
                Command cmd = _parser.ParseInputed(userInput);
                Task task = _assignor.Assign(_workingDirectoryManager.Get(),userInput, cmd);
                
                if (task.TaskType == TaskType.BUILTIN)
                {
                    var result = _builtinManager.AssignBuiltin(_workingDirectoryManager, task);
                    if (result != "")
                        Console.Write(result);
                }
                else if (task.TaskType == TaskType.NOOP) {}
                else
                {
                    Console.WriteLine("this task is not supporting.");
                }
            }
        }


        // public string AssignBuiltin(Task task)
        // {
        //     // ビルトインの割り当て"ls"しかしてないし、プロセスマネジャの設定もしてないので"ls"しか動かん。
        //     switch (task.Command.Fn)
        //     {
        //         case "ls":
        //         {
        //             // [0]は一時的な処理、つまり妥協。めんどくさい。直すの。(><)
        //             return Ls(task.Command.Args, task.Command.Ops);
        //         }
        //         case "pwd":
        //         {
        //             return $"{Pwd(task.Command.Args, task.Command.Ops)}\n";
        //         }
        //
        //         case "cd":
        //         {
        //             
        //             return "";
        //         }
        //         
        //     }
        //     return $"error: `{task.Command.Fn}`\nmsg: this built-in function is not assigned yet.\n";
        // }
        
        
        // ------------------------------------------------------------------------
        // プロンプト
        
        public string ExchangePromptFormat(string format)
        {
            switch (format)
            {
                case "%whoami":
                {
                    return BuiltinManager.WhoAmI();
                }

                case "%whereami_name":
                {
                    var whereami = _workingDirectoryManager.Get();
                    var whereamiRemovedLastSlash = whereami.Substring(0, whereami.Length - 1);
                    var lastSlash = whereamiRemovedLastSlash.LastIndexOf("/", StringComparison.Ordinal);
                    return whereami.Substring(lastSlash+1, whereamiRemovedLastSlash.Length-lastSlash-1);
                }

                case "%whereami_full":
                {
                    return _workingDirectoryManager.Get();;
                }
            }

            return format;
        }

        public string GenPrompt()
        {
            var promptStyle = _settingsManager.GetPromptStyle();
            List<string> supportingFormat = new List<string>() { "%whoami", "%whereami_name", "%whereami_full" };

            foreach (var format in supportingFormat)
            {
                promptStyle = promptStyle.Replace(format, ExchangePromptFormat(format));
            }
            
            return promptStyle;
        }
        
        
        // ------------------------------------------------------------------------
        // CMDs
        
        // use /usr/bin/whoami
        // public string WhoAmI()
        // {
        //     var whoami = new WhoAmI();
        //     return whoami.GetUserName();
        // }
        //
        // public string Ls(List<string> args, List<string> ops)
        // {
        //     
        //     // sortが必要そう。
        //     // なんか他のだと普通、横に並べてる。
        //     string _args;
        //     
        //     // 期待するもの
        //     // List<string> args, args.Count() => 1
        //     
        //     // 引数なしの場合
        //     if (!args.Any())
        //         _args = "";
        //     // 引数多すぎ問題
        //     else if (args.Count > 1)
        //         throw new Exception("too many arguments");
        //     else
        //         _args = args[0];
        //
        //     var ls = new Ls();
        //     return ls.GetFileAndDirectoryNameOnlyPrint(WhereAmI, _args, ops);
        // }
        //
        // public string Cd(List<string> args, List<string> ops)
        // {
        //     // todo : 引数チェック関数の導入
        //     string _args;
        //     if (!args.Any())
        //         _args = "";
        //     // 引数多すぎ問題
        //     else if (args.Count > 1)
        //         throw new Exception("too many arguments");
        //     _args = args[0];
        //     
        //     var cd = new Cd();
        //     // WhereAmI = cd.Move(WhereAmI, path);// 時間かかる。
        //     WhereAmI = cd.MoveQuick(WhereAmI, _args);
        //     return WhereAmI;
        // }
        //
        // public string Pwd(List<string> args, List<string> ops)
        // {
        //     return WhereAmI;
        // }
        //
        // public string Cat(string path)
        // {
        //     return "";
        // }
        
    }
}