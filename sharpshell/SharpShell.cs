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
using sharpshell.virtualpath;
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
        private VirtualPathManager _virtualPathManager;
        private BuiltinManager _builtinManager;
        private ProcessManager _process;
        // その他
        private JsonLoader.Loader _jsonLoader = new JsonLoader.Loader();

        
        public SharpShell(string entryPoint)
        {
            _virtualPathManager = new VirtualPathManager(entryPoint);
            _settingsManager = new SettingsManager("/Users/x0y14/dev/csharp/sharpshell/sharpshell/setting.json");
            _listener = new Listener();
            _parser = new Parser();
            _assignor = new Assignor(_settingsManager.GetPath());
            _builtinManager = new BuiltinManager();
            _process = new ProcessManager();
            Init();
            LineStreaming();
        }

        private void Init()
        {
            // 設定読み込み
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
                Task task = _assignor.Assign(_virtualPathManager,userInput, cmd);
                
                if (task.TaskType == TaskType.BUILTIN)
                {
                    var result = _builtinManager.AssignBuiltin(_virtualPathManager, task);
                    if (result != "")
                        Console.Write(result);
                }
                else if (task.TaskType == TaskType.UNKNOWN)
                {
                    var result = _process.MakeBinaryProcess(task.BinPath, task);
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
        // ------------------------------------------------------------------------
        // プロンプト生成はデザイナーに任せるかも？
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
                    var whereami = _virtualPathManager.GetWorkingDirectoryAsString();
                    var whereamiRemovedLastSlash = whereami.Substring(0, whereami.Length - 1);
                    var lastSlash = whereamiRemovedLastSlash.LastIndexOf("/", StringComparison.Ordinal);
                    return whereami.Substring(lastSlash+1, whereamiRemovedLastSlash.Length-lastSlash-1);
                }

                case "%whereami_full":
                {
                    return _virtualPathManager.GetWorkingDirectoryAsString();;
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
    }
}