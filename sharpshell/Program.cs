using System;
using System.Collections.Generic;
using System.Diagnostics;
using JsonLoader;
using sharpshell.lib;

namespace sharpshell
{
    class Program
    {
        static void Main(string[] args)
        {
            var jl = new JsonLoader.Loader();
            var settings = jl.LoadWithPath("/Users/x0y14/dev/csharp/sharpshell/sharpshell/setting.json");
            var sh = new SharpShell();
            Console.WriteLine($"user name is `{sh.WhoAmI()}`");
            // sh.Ls();
            Console.WriteLine($"now {sh.Pwd()}");
            sh.Cd("Qt");
            Console.WriteLine($"moved {sh.Pwd()}");

            Dictionary<string, dynamic> fd = sh.Ls(sh.Pwd(), "");
        }
    }
}