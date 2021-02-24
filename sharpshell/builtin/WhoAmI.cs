using System;
using sharpshell.process;
using sharpshell.misc;

namespace sharpshell.builtin
{
    public class WhoAmI
    {
        private readonly ProcessManager _process;
        public WhoAmI()
        {
            _process = new ProcessManager();
        }
        
        public string GetUserNameUserBin()
        {
            var _userName = _process.MakeBinaryProcess("/usr/bin/whoami", "");
            return Util.RemoveLastNewLine(_userName);
        }

        public string GetUserName()
        {
            return Environment.UserName;
        }
    }
}