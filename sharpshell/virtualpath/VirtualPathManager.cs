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
    /// ls, mkdir, cd, ... パスに関する動作の根本を担うものなので結構重要。
    /// こいつが自発的に動くことはない.
    /// こいつの役割は, 文字列で入力されたパスらしきものを、解析すること.
    /// 指示があった場合、そこに動けるかを確認したのちに、可能であれば動くこと.
    /// </summary>
    public class VirtualPathManager
    {

        private string[] _workingDirectory;
        public VirtualPathManager(string entryPoint)
        {
            // 絶対パス
            _workingDirectory = ConvertPathStringToArray(entryPoint);
        }

        private static string[] ConvertPathStringToArray(string path)
        {
            var pathList = new List<string>();
            
            // Q. 絶対パスを扱うので"/"が入ってないのはおかしいのでは？
            // A. 移動の際に相対パスを使うのでありうる。
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
            foreach (var dir in _workingDirectory)
            {
                wd += $"/{dir}";
            }
            return wd;
        }

        public string PathArrayToString(string[] path)
        {
            var wd = "";
            foreach (var dir in path)
            {
                wd += $"/{dir}";
            }
            return wd;
        }

        public string GoUp()
        {
            var l = _workingDirectory.ToList();
            var nl = l.GetRange(0, l.Count-1);
            _workingDirectory = nl.ToArray();
            return GetWorkingDirectoryAsString();
        }
        
        public string GoDown(string dirName)
        {
            if (!IsExistDirectoryFromDirName(dirName))
                throw new Exception($"no such directory: {dirName}");
            var l = _workingDirectory.ToList(); 
            l.Add(dirName);
            _workingDirectory = l.ToArray();
            return GetWorkingDirectoryAsString();
        }
        
        public string SetAbsolutePath(string newPath)
        {
            // newPathが正しいかどうかは検証しない。
            // CDから呼び出されることのみを想定している。
            // CDの方では存在確認をしている。
            _workingDirectory = ConvertPathStringToArray(newPath);
            return GetWorkingDirectoryAsString();
        }

        public static VirtualPath IsExist(string absolutePath)
        {
            if (Directory.Exists(absolutePath))
                return new VirtualPath(VirtualPathType.DIRECTORY, absolutePath);
            if (File.Exists(absolutePath))
                return new VirtualPath(VirtualPathType.FILE, absolutePath);
            return new VirtualPath(VirtualPathType.NOTFOUND, absolutePath);
        }

        public bool IsExistDirectoryFromDirName(string dirName)
        {
            return Directory.Exists($"{GetWorkingDirectoryAsString()}/{dirName}");
        }

        public bool IsExistFileFromFileName(string fileName)
        {
            return File.Exists($"{GetWorkingDirectoryAsString()}/{fileName}");
        }
        
        // public VirtualPathManager CloneAsTemp()
        // {
        //     return new VirtualPathManager(GetWorkingDirectoryAsString());
        // }

        public string[] AttachRelativePathToTemp(string[] relativePath)
        {
            var newPath = _workingDirectory.ToList();
            newPath.AddRange(relativePath.ToList());
            return newPath.ToArray();
        }
        
        // ---------------------------------------------------
        // 相対パス
        private VirtualPath AnalyzeRelativePath(string path)
        {
            // ^((?:\.\.\/)+)(.*)$
            // 上のは(../)+(.*)みたいな
            
            if (path == "" || path == ".")
            {
                // 現在地がディレクトリ以外にいるってどういうことなんだけども一応調べる？
                return new VirtualPath(VirtualPathType.DIRECTORY, GetWorkingDirectoryAsString());
            }
            
            if (path.StartsWith(".."))
            {
                // var tempRelative = CloneAsTemp();
                
                // どのくらい下がるのか、
                var splitPathArray = ConvertPathStringToArray(path);
                var relative = new List<string>();
                foreach (var p in splitPathArray)
                {
                    if (p == "..")
                    {
                        // 解析と同時に上がっちゃう
                        GoUp();
                    }
                    else
                    {
                        relative.Add(p);
                    }
                }
                var unknownPathArrayRelative = AttachRelativePathToTemp(relative.ToArray());
                var unknownPathRelative = PathArrayToString(unknownPathArrayRelative);
                return IsExist(unknownPathRelative);
                
            }

            
            // .//////dev, ./dev, dev, は同じなので./を取り除く。
            var sameFloorPathReg = new Regex(@"^\.(?:/)+(.*)?$", RegexOptions.Compiled);
            MatchCollection matches = sameFloorPathReg.Matches(path);
            if (matches.Count == 1)
            {
                GroupCollection r = matches[0].Groups;
                path = r[1].Value;
            }
            
            // Console.WriteLine($"path: {path}");
            // これがなんなのか
            var unknownPathArrayAbsolute = AttachRelativePathToTemp(ConvertPathStringToArray(path));
            var unknownPathAbsolute = PathArrayToString(unknownPathArrayAbsolute);
            return IsExist(unknownPathAbsolute);
        }
        
        // ---------------------------------------------------
        // 絶対パス
        private static VirtualPathType GetVirtualPathTypeFromAbsolutePath(string path)
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
            if (path.StartsWith("/"))
            {
                // 絶対パス。
                return AnalyzeAbsolutePath(path);
            }
            // if (path.Substring(path.ength - 1, 1) == "/")
            // path = paLth.Substring(0, path.Length - 1);
            return AnalyzeRelativePath(path);
        }

    }
}