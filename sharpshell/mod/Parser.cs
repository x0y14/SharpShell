using System;
using System.Collections.Generic;
using System.Linq;
using sharpshell.rule;

namespace sharpshell.mod
{
    public class Parser
    {
        public Parser(){}

        public Command ParseInputed(string input)
        {
            // 引数なし, オプションなし.
            if (!input.Contains(" "))
            {
                return new Command(input, new List<string>(), new List<string>());
            }
            // 引数あり, オプションなし.
            if (!input.Contains("-"))
            {
                var splited = input.Split(" ").ToList();
                var fnName = splited[0];
                var args = splited.GetRange(1, splited.Count-1);
                return new Command(fnName, new List<string>(), args);
            }
            
            // todo : まともなパース。
            
            return new Command();
        }
    }
}