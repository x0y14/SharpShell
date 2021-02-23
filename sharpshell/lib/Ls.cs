using System;
using System.Collections.Generic;
using sharpshell.process;
using sharpshell.misc;

namespace sharpshell.lib
{
    public class Ls
    {
        private readonly ProcessManager _process;
        public Ls()
        {
            _process = new ProcessManager();
        }
        
        
        
        public Dictionary<string, dynamic> GetFileAndDictionary(string path, string args)
        {
            // これ/bin/lsに頼らないで自分で書くのもありかも。
            // does not support args
            var fileAndDirs = new List<string>();
            var files = new List<string>();
            var dirs = new List<string>();

            var result = _process.MakeBinaryProcess("/bin/ls", $"{path} {args}");
            result = Util.RemoveLastNewLine(result);
            var items = result.Split("\n");
            
            var fi = 0;
            foreach (var f in items)
            {
                fi++;
                fileAndDirs.Add(f);
                
                #if DEBUG
                Console.WriteLine($"({fi}) {f}");
                #endif
                
                if (System.IO.Directory.Exists($"{path}/{f}"))
                {
                    dirs.Add(f);
                }
                else
                {
                    files.Add(f);
                }
            }
            
            // ShellLogging(UserName, WhereAmI, "SharpShell.Ls", path);

            var data = new Dictionary<string, dynamic>();
            data["files"] = files;
            data["dirs"] = dirs;
            data["all"] = fileAndDirs;
            return data;
        }
    }
}