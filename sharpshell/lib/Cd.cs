using System;
using sharpshell.lib;

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
            string newPath;
            var thisFloorsItem = _ls.GetFileAndDictionary(whereami, "");
            if (thisFloorsItem["dirs"].Contains(path))
            {
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