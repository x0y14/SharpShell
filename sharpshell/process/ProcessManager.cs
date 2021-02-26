using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using sharpshell.rule;

namespace sharpshell.process
{
    public class ProcessManager
    {
        public ProcessManager()
        {
        }
        
        public string MakeBinaryProcess(ShellTask task)
        {
            // 外部のソフトを使う時に使う。
            // return value includes last newline.
            ProcessStartInfo process = new ProcessStartInfo(task.BinPath, "")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            };

            Process p = new Process {StartInfo = process};
            p.Start();
            return p.StandardOutput.ReadToEnd();
        }

        public string MakeBinaryProcess2(ShellTask task)
        {
            var si = new ProcessStartInfo(task.BinPath, task.RawInput.Replace(task.Command.Fn, ""));
            si.RedirectStandardError = true;
            si.RedirectStandardOutput = true;
            si.CreateNoWindow = true;
            si.UseShellExecute = false;
            using (var proc = new Process())
            // using (var ctoken = new CancellationTokenSource())
            {
                proc.EnableRaisingEvents = true;
                proc.StartInfo = si;
                // コールバックの設定
                proc.OutputDataReceived += (sender, ev) =>
                {
                    // Console.Write(proc.CancelOutputRead());
                    Console.WriteLine(ev.Data);
                    // proc.CancelOutputRead();
                };
                proc.ErrorDataReceived += (sender, ev) =>
                {
                    Console.WriteLine(ev.Data);
                    // Console.Write(proc.StandardError.ReadLine());
                    // proc.CancelErrorRead();
                };
                proc.Exited += (sender, ev) =>
                {
                    Console.WriteLine($"exited");
                    // プロセスが終了すると呼ばれる
                };
                // プロセスの開始
                proc.Start();
                // 非同期出力読出し開始
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
                // 終了まで待つ
                // ctoken.Token.WaitHandle.WaitOne();
                proc.WaitForExit();
            }

            return "";
        }
    }
}