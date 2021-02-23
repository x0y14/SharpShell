using System;
using System.Collections.Generic;
using sharpshell.rule;

namespace sharpshell.mod
{
    public class Parser
    {
        public Parser(){}

        public Command ParseInputed(string input)
        {
            if (!input.Contains(" "))
            {
                return new Command(input, new List<string>(), new List<string>());
            }
            
            // todo : まともなパース。
            
            return new Command();
        }
    }
}