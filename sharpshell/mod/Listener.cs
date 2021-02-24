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
        public Listener() {}

        public string Listening(string prompt)
        {
            Console.Write(prompt);
            return System.Console.ReadLine();;
        }
    }
}