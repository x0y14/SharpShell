using System;
using sharpshell.process;
using sharpshell.misc;
using sharpshell.rule;

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
            var whoami = new ShellTask(
                ShellTaskType.BUILTIN,
                "",
                new Command(),
                "/usr/bin/whoami"
                );
            var _userName = _process.MakeBinaryProcess(whoami);
            return Util.RemoveLastNewLine(_userName);
        }

        public string GetUserName()
        {
            return Environment.UserName;
        }
    }
}