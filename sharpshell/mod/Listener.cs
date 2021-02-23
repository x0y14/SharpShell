using System;
using sharpshell.rule;

namespace sharpshell.mod
{
    /// <summary>
    /// リスニング、コマンドの割り当てを担当する。結構重役。
    /// まぁ割り当ては別に分けても良いと思う。
    /// </summary>
    public class Listener
    {
        private Parser _parser;

        public Listener()
        {
            _parser = new Parser();
        }

        public Command Listening(string prompt)
        {
            Console.Write(prompt);
            // なぜか知らんが最後の改行は含まれない。
            var line = System.Console.ReadLine();
            return _parser.ParseInputed(line);
        }
    }
}