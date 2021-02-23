using System.Collections.Generic;

namespace sharpshell.rule
{
    public readonly struct Command
    {
        public readonly string Fn;
        public readonly List<string> Ops;
        public readonly List<string> Args;

        public Command(string fn, List<string> ops, List<string> args)
        {
            Fn = fn;
            Ops = ops;
            Args = args;
        }
    }
}