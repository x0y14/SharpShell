using System;
using System.Diagnostics;
using JsonLoader;

namespace sharpshell
{
    class Program
    {
        static void Main(string[] args)
        {
            var jl = new JsonLoader.Loader();
            var settings = jl.LoadWithPath("/Users/x0y14/dev/csharp/sharpshell/sharpshell/setting.json");
            var sh = new SharpShell(settings);
            Console.WriteLine($"user name is `{sh.WhoAmI()}`");
            // sh.Ls();
            Console.WriteLine($"now {sh.Pwd()}");
            sh.Cd("Qt");
            Console.WriteLine($"now {sh.Pwd()}");
            // sh.Ls();
        }
    }
}