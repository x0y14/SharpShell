using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using sharpshell.rule;
using sharpshell.virtualpath;

namespace sharpshell.builtin
{
    public class Cd
    {
        /// <summary>
        /// CDはオプションチェックの関数！！！
        /// </summary>
        private Ls _ls;
        private VirtualPathManager _virtualPathManager;
        public Cd(VirtualPathManager wdm)
        {
            _ls = new Ls();
            _virtualPathManager = wdm;
        }
        
        public string MoveQuick(string path)
        {
            // 末尾が/だった場合、それを消す。
            
            // 引数なし、動かない。
            if (path == "")
                return _virtualPathManager.GetWorkingDirectoryAsString();

            var virtualPath = _virtualPathManager.AnalyzePath(path);
            if (virtualPath.Type == VirtualPathType.DIRECTORY)
            {
                return _virtualPathManager.SetAbsolutePath(virtualPath.AbsolutePath);
            }
            if (virtualPath.Type == VirtualPathType.FILE)
                throw new Exception($"not a directory: {path}\n");
            throw new Exception($"not found: {path}\n");


            // if (path == "..")
            // {
            //     _workingDirectoryManager.GoUp();
            //     return;
            // }

            // if (_workingDirectoryManager.IsExistFile(path))
            //     throw new Exception($"not a directory: {path}");
            //
            // try
            // {
            //     _workingDirectoryManager.GoDown(path);
            //     return;
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine(e);
            //     throw new Exception($"not a directory: {path}");
            // }

            // 同じ階層に配置されたディレクトリを指定された場合。
            // if (Directory.Exists($"{_workingDirectoryManager.GetWorkingDirectoryAsString()}/{path}"))
            //     Console.WriteLine("");
            // return $"{_workingDirectoryManager.GetWorkingDirectoryAsString()}/{path}";
            // 
            // 相対ぱす

        }
    }
}