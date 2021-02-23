using System;
using System.Collections.Generic;

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
        
        public static List<string> ConvertPathList(List<object> paths)
        {
            var converted = new List<string>();
            
            foreach (var path_o in paths)
            {
                string path;
                try
                {
                    path = path_o.ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                converted.Add(CheckPathTail(path));
            }
            
            return converted;
        }
        
        public static string CheckPathTail(string path)
        {
            if (path.Substring(path.Length - 1, 1) == "/")
                return path;
            return $"{path}/";
        }
    }
}