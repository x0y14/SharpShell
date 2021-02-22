using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using sharpshell.misc;
using static sharpshell.Logger;
using static sharpshell.misc.Util;

namespace sharpshell
{
    public class SharpShell
    {
        private string UserName;
        private string WhereAmI;

        public SharpShell(Dictionary<string, dynamic> settings)
        {
            UserName = WhoAmI();
            WhereAmI = $"/Users/{WhoAmI()}";
            // settings
        }
        
        public string ProcessMaker(string bin_path, string args)
        {
            // return value includes last newline.
            ProcessStartInfo process = new ProcessStartInfo(bin_path, args)
            {
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            Process p = new Process {StartInfo = process};
            p.Start();
            return p.StandardOutput.ReadToEnd();
        }
        
        // use /usr/bin/whoami
        public string WhoAmI()
        {
            var _userName = ProcessMaker("/usr/bin/whoami", "");
            ShellLogging(UserName, WhereAmI, "SharpShell.WhoAmI", "");
            return RemoveLastNewLine(_userName);
        }

        // use /bin/ls
        public Dictionary<string, dynamic> Ls(string path="")
        {
            // does not support args
            var fileAndDirs = new List<string>();
            var files = new List<string>();
            var dirs = new List<string>();
            
            
            if (path == "")
            {
                path = WhereAmI;
            }
            
            var result = ProcessMaker("/bin/ls", path);
            result = RemoveLastNewLine(result);
            var items = result.Split("\n");
            
            var fi = 0;
            foreach (var f in items)
            {
                fi++;
                fileAndDirs.Add(f);
                
                #if DEBUG
                Console.WriteLine($"({fi}) {f}");
                #endif
                
                if (System.IO.Directory.Exists($"{WhereAmI}/{f}"))
                {
                    dirs.Add(f);
                }
                else
                {
                    files.Add(f);
                }
            }
            
            ShellLogging(UserName, WhereAmI, "SharpShell.Ls", path);

            var data = new Dictionary<string, dynamic>();
            data["files"] = files;
            data["dirs"] = dirs;
            data["all"] = fileAndDirs;
            return data;
        }

        public bool Cd(string path)
        {
            var thisFloorsItem = Ls();
            if (thisFloorsItem["dirs"].Contains(path))
            {
                WhereAmI += $"/{path}";
            }
            else
            {
                Console.WriteLine($"{path} is not dir");
            }
            
            return true;
        }

        public string Pwd()
        {
            return WhereAmI;
        }
        
    }
}