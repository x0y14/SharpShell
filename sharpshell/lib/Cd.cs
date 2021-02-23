using System;

namespace sharpshell.lib
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
            string newPath;
            var thisFloorsItem = _ls.GetFileAndDirectoryNameOnly(whereami, whereami, "");

            if (thisFloorsItem["d"].Contains(path))
            {
                // (#1)
                newPath = whereami + $"/{path}";
            }
            else
            {
                throw new Exception($"{path} is not dir");
            }
            
            return newPath;
        }
    }
}