using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

using sharpshell.lib;
using sharpshell.process;
using sharpshell.mod;
using sharpshell.misc;

using static sharpshell.misc.Util;

namespace sharpshell
{
    public class SharpShell
    {
        private string UserName;
        private string WhereAmI;
        private Dictionary<string, dynamic> Settings;
        private string PromptStyle;

        private Listener _listener;
        private ProcessManager _process;
        
        private JsonLoader.Loader _jsonLoader = new JsonLoader.Loader();

        public SharpShell()
        {
            UserName = WhoAmI();
            WhereAmI = $"/Users/{WhoAmI()}/";

            _listener = new Listener();
            _process = new ProcessManager();
            
            Init();
            LineStreaming();
        }

        private void Init()
        {
            LoadSettings("/Users/x0y14/dev/csharp/sharpshell/sharpshell/setting.json");
            
            if (Settings.ContainsKey("welcome-message"))
                Console.WriteLine(Settings["welcome-message"]);
        }

        private void LineStreaming()
        {
            // sharminal側で無限ループさせるのもありかも
            while (true)
            {
                _listener.Listening(GenPrompt());
            }
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
        }

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

        private string Listen()
        {
            return "";
        }
        
        // ----------------------------------------------------------------
        // CMDs
        
        // use /usr/bin/whoami
        public string WhoAmI()
        {
            var whoami = new WhoAmI();
            return whoami.GetUserName();
        }
        
        public Dictionary<string, dynamic> Ls(string path, string args)
        {
            var ls = new Ls();
            return ls.GetFileAndDirectoryNameOnly(WhereAmI, path, args);
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