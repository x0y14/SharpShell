using sharpshell.process;
using sharpshell.misc;

namespace sharpshell.lib
{
    public class WhoAmI
    {
        private readonly ProcessManager _process;
        public WhoAmI()
        {
            _process = new ProcessManager();
        }
        
        public string getUserName()
        {
            var _userName = _process.MakeBinaryProcess("/usr/bin/whoami", "");
            return Util.RemoveLastNewLine(_userName);
        }
    }
}