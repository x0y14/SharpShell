using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

using sharpshell.lib;
using sharpshell.process;
using sharpshell.misc;

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
        }

        public void LoadSettings(string path)
        {
            
            // Settings = settings;
        }
        
        
        // ----------------------------------------------------------------
        // CMDs
        
        // use /usr/bin/whoami
        public string WhoAmI()
        {
            var whoami = new WhoAmI();
            return whoami.getUserName();
        }
        
        public Dictionary<string, dynamic> Ls(string path, string args)
        {
            var ls = new Ls();
            return ls.GetFileAndDirectoryNameOnly(WhereAmI, path, args);
        }
        
        public string Cd(string path)
        {
            var cd = new Cd();
            WhereAmI = cd.Move(WhereAmI, path);
            return WhereAmI;
        }
        
        public string Pwd()
        {
            return WhereAmI;
        }
        
    }
}