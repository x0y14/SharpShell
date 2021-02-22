using System;

namespace sharpshell.misc
{
    public class Util
    {
        public static string RemoveLastNewLine(string data)
        {
            // センシティブな内容を扱うことがあるので、セキュアにしたい。
            if (data.Substring(data.Length-1, 1) != "\n")
                return data;
            return data.Substring(0, data.Length-1);
        }
    }
}