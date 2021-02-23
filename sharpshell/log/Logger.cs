using System;


namespace sharpshell.log
{
    public class Logger
    {
        public static void ShellLogging(string userName, string path, string cmd, string args)
        {
            #if DEBUG
            Console.WriteLine($"{DateTime.Now} | {path} | {userName} | {cmd} | {args}");
            #endif
            // todo : fileに保存
            
        }

        public static void NoticeLog(string type, string data)
        {
            Console.WriteLine($"[{type}] {data}");
        }
    }
}