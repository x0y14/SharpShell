using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace sharpshell.builtin
{
    public class Cd
    {
        private Ls _ls;
        public Cd()
        {
            _ls = new Ls();
        }
        
        public string Move(string whereami, string path)
        {
            // todo : 相対ぱすに対応。
            // 現段階では、GetFileAndDirectoryNameOnlyにDirと認識されているもののみに反応する。(#1)
            // とゆかクソ遅い。
            string newPath;
            var thisFloorsItem = _ls.GetFileAndDirectoryNameOnly(whereami, whereami, new List<string>());

            if (thisFloorsItem["d"].Contains(path))
            {
                // (#1)
                newPath = whereami + $"{path}/";
            }
            else
            {
                throw new Exception($"{path} is not dir");
            }
            
            return newPath;
        }

        public string MoveQuick(string whereami, string path)
        {
            // todo : 相対ぱすに対応。
            // 超一部だけ相対ぱすに対応。
            
            // "Qt/" -> "Qt"
            if (path.Substring(path.Length - 1, 1) == "/")
                path = path.Substring(0, path.Length - 1);
            
            // 引数なし、動かない。
            if (path == "" || path == ".")
                return whereami;
            
            // 同じ階層の場合("./" or "." or "./////////////////////////...")
            var thisFloorPathReg = new Regex(@"^\./+", RegexOptions.Compiled);
            MatchCollection match = thisFloorPathReg.Matches(path);
            if (match.Count == 1)
            {
                return whereami;
            }
            if (Directory.Exists($"{whereami}{path}/"))
                return $"{whereami}{path}/";
            if (File.Exists($"{whereami}{path}"))
                throw new Exception($"{whereami}{path} is not Directory.");
            
            // 相対ぱす

            throw new Exception($"not found: {path}");
        }
    }
}