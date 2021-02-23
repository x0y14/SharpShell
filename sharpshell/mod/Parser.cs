using System;
using System.Collections.Generic;
using sharpshell.rule;

namespace sharpshell.mod
{
    public class Parser
    {
        public Parser(){}

        public Command AnalyzeInputed(string input)
        {
            Console.WriteLine($"[INFO] inputed: `{input}`");
            if (input.Contains(" "))
            {
                return new Command(input, new List<string>(), new List<string>());
            }
            
            return new Command();
        }
    }
}