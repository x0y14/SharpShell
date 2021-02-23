using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

using sharpshell.lib;
using sharpshell.process;
using sharpshell.misc;
using static sharpshell.Logger;

using static sharpshell.misc.Util;

namespace sharpshell
{
    public class SharpShell
    {
        private string UserName;
        private string WhereAmI;
        private Dictionary<string, dynamic> Settings;

        private ProcessManager _process;
        
        private JsonLoader.Loader _jsonLoader = new JsonLoader.Loader();

        public SharpShell()
        {
            UserName = WhoAmI();
            WhereAmI = $"/Users/{WhoAmI()}";

            _process = new ProcessManager();

            // settings

            // if (settings.ContainsKey("welcome-message")){}
        }

        public void LoadSettings(string path)
        {
            
            // Settings = settings;
        }
        
        // use /usr/bin/whoami
        public string WhoAmI()
        {
            var whoami = new WhoAmI();
            return whoami.getUserName();
        }

        // use /bin/ls
        public Dictionary<string, dynamic> Ls(string path, string args)
        {
            var ls = new Ls();
            return ls.GetFileAndDictionary(path, args);
        }
        
        public bool Cd(string path)
        {
            // boolにすべきか？
            // 新しいpathを返すべきか
            var state = false;
            var _cd = new Cd();
            try
            {
                WhereAmI = _cd.Move(WhereAmI, path);
                state = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return state;
        }

        public string Pwd()
        {
            #if DEBUG
            ShellLogging(UserName, WhereAmI, "SharpShell.Pwd", "");
            #endif
            return WhereAmI;
        }
        
    }
}