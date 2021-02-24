using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace sharpshell.builtin
{
    public class Ls
    {
        public Ls() {}
        
        public string RemoveTrashFromPath(string whereami, string path)
        {
            var cleaned_path = path;
            
            var regText = $"{whereami}{{1,1}}(.*)";
            var reg = new Regex(regText, RegexOptions.Compiled);
            
            MatchCollection matches = reg.Matches(path);
            
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                cleaned_path = groups[1].ToString();
            }

            return cleaned_path;
        }

        public List<string> GetDirs(string path)
        {
            var dirs = new List<string>();
            return Directory.GetDirectories(path).ToList();
        }

        public List<string> GetFiles(string path)
        {
            var files = new List<string>();
            return Directory.GetFiles(path).ToList();
        }

        public Dictionary<string, dynamic> GetFileAndDirectoryNameOnly(string whereami, string path, List<string> ops)
        {
            if (path == "")
            {
                path = whereami;
            }
            // 自作してみる。
            var fileAndDirs = new Dictionary<string, dynamic>();
            var files = GetFiles(path);
            var dirs = GetDirs(path);

            var fileNames = new List<string>();
            var dirNames = new List<string>();

            foreach (var file in files)
            {
                fileNames.Add(RemoveTrashFromPath(whereami,file));
            }

            foreach (var dir in dirs)
            {
                dirNames.Add(RemoveTrashFromPath(whereami,dir));
            }

            fileAndDirs["f"] = fileNames;
            fileAndDirs["d"] = dirNames;
            
            return fileAndDirs;
        }

        public string GetFileAndDirectoryNameOnlyPrint(string whereami, string path, List<string> ops)
        {
            string resultText = "";
            var fileAndDir = GetFileAndDirectoryNameOnly(whereami, path, ops);
            foreach (var file in fileAndDir["f"])
            {
                resultText += $"{file}\n";
            }
            
            foreach (var dir in fileAndDir["d"])
            {
                resultText += $"{dir}\n";
            }
            
            return resultText;
        }
        
        
    }
}