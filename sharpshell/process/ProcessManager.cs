using System.Diagnostics;

namespace sharpshell.process
{
    public class ProcessManager
    {
        public ProcessManager(){}
        
        public string MakeBinaryProcess(string bin_path, string args)
        {
            // lsなど外部のソフトを使う時に使う。
            // return value includes last newline.
            ProcessStartInfo process = new ProcessStartInfo(bin_path, args)
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