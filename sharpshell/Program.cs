using System;
using System.Collections.Generic;
using System.Diagnostics;
using JsonLoader;
using sharpshell.builtin;

namespace sharpshell
{
    class Program
    {
        static void Main(string[] args)
        {
            // todo : テストをかく。(切実)
            var sh = new SharpShell();
            
            // Console.WriteLine(sh.GenPrompt());
            // sh.Cd("Qt");
            // Console.WriteLine(sh.GenPrompt());
            // Console.WriteLine(sh.Ls(".", ""));
            //
            // Console.WriteLine($"user name is `{sh.WhoAmI()}`");
            // // sh.Ls();
            // Console.WriteLine($"now `{sh.Pwd()}`");
            // sh.Cd("Qt");
            // Console.WriteLine($"moved `{sh.Pwd()}`");
            // Console.WriteLine(Environment.UserName);
        }
    }
}