using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;
using sharpshell.rule;

namespace sharpshell.process
{
    public class ProcessManager
    {
        public ProcessManager(){}
        
        public string MakeBinaryProcess(string bin_path, Task task)
        {
            // 外部のソフトを使う時に使う。
            // return value includes last newline.
            ProcessStartInfo process = new ProcessStartInfo(bin_path, "")
            {
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            Process p = new Process {StartInfo = process};
            p.Start();
            return p.StandardOutput.ReadToEnd();
        }
    }
}