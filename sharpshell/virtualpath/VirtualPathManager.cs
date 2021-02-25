using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using sharpshell.mod;
using sharpshell.rule;
using Path = System.IO.Path;

namespace sharpshell.virtualpath
{
    /// <summary>
    /// こいつが自発的に動くことはない.
    /// こいつの役割は, 文字列で入力されたパスらしきものを、解析すること.
    /// 指示があった場合、そこに動けるかを確認したのちに、可能であれば動くこと.
    /// </summary>
    public class VirtualPathManager
    {

        private string[] _virtualPathManagerStrings;
        public VirtualPathManager(string now)
        {
            _virtualPathManagerStrings = ConvertPathStringToArray(now);
        }

        private string[] ConvertPathStringToArray(string path)
        {
            var pathList = new List<string>();
            
            // スラッシュが含まれないことってあるの？
            if (!path.Contains("/"))
            {
                return new[] {path};
            }
            
            foreach (var splitPath in path.Split("/"))
            {
                if (splitPath != "")
                    pathList.Add(splitPath);
            }
            
            return pathList.ToArray();
        }

        public string GetWorkingDirectoryAsString()
        {
            var wd = "";
            
            foreach (var dir in _virtualPathManagerStrings)
            {
                wd += $"/{dir}";
            }
            return wd;
        }

        public string GoUp()
        {
            var l = _virtualPathManagerStrings.ToList();
            var nl = l.GetRange(0, l.Count-1);
            _virtualPathManagerStrings = nl.ToArray();
            return GetWorkingDirectoryAsString();
        }
        
        public string GoDown(string dirName)
        {
            if (!IsExistDirectory(dirName))
                throw new Exception($"no such directory: {dirName}");
            var l = _virtualPathManagerStrings.ToList(); 
            l.Add(dirName);
            _virtualPathManagerStrings = l.ToArray();
            return GetWorkingDirectoryAsString();
        }
        
        public string SetAbsolutePath(string newPath)
        {
            // newPathが正しいかどうかは検証しない。
            // CDから呼び出されることのみを想定している。
            // CDの方では存在確認をしている。
            _virtualPathManagerStrings = ConvertPathStringToArray(newPath);
            return GetWorkingDirectoryAsString();
        }

        public bool IsExistDirectory(string dirName)
        {
            return Directory.Exists($"{GetWorkingDirectoryAsString()}/{dirName}");
        }

        public bool IsExistFile(string fileName)
        {
            return File.Exists($"{GetWorkingDirectoryAsString()}/{fileName}");
        }
        
        public VirtualPathManager CloneAsTemp()
        {
            return new VirtualPathManager(GetWorkingDirectoryAsString());
        }
        
        private string AnalyzeRelativePath(string relativePath)
        {
            // ^((?:\.\.\/)+)(.*)$
            // || path == "."

            if (relativePath == ".")
            {
                return "";
            }
            
            // 同じ階層の場合("./" or "." or "./////////////////////////...")
            var thisFloorPathReg = new Regex(@"^\./+", RegexOptions.Compiled);
            MatchCollection match = thisFloorPathReg.Matches(relativePath);
            if (match.Count == 1)
            {
                return "";
            }

            return "";
        }

        
        // ---------------------------------------------------
        // 絶対パス
        private VirtualPathType GetVirtualPathTypeFromAbsolutePath(string path)
        {
            if (Directory.Exists(path))
                return VirtualPathType.DIRECTORY;
            if (File.Exists(path))
                return VirtualPathType.FILE;
            return VirtualPathType.NOTFOUND;
        }

        private VirtualPath AnalyzeAbsolutePath(string path)
        {
            var type = GetVirtualPathTypeFromAbsolutePath(path);
            return new VirtualPath(type, path);
        }
        
        public VirtualPath AnalyzePath(string path)
        {
            VirtualPath result;
            
            if (path.StartsWith("/"))
            {
                // 絶対パス。
                return AnalyzeAbsolutePath(path);
            }
            
            // if (path.Substring(path.ength - 1, 1) == "/")
            // path = paLth.Substring(0, path.Length - 1);
            return new VirtualPath(VirtualPathType.NOTFOUND, "");
        }

    }
}