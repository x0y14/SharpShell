using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using sharpshell.rule;
using sharpshell.wd;

namespace sharpshell.builtin
{
    public class BuiltinManager
    {
        public BuiltinManager(){}
        public static readonly List<string> SupportingCommands = new List<string>(){"ls", "cat", "cd", "pwd", "whoami", "clear"};// ?

        public static bool IsSupporting(string cmd)
        {
            return SupportingCommands.Contains(cmd);
        }

        private string DoInSandBox(Func<WorkingDirectoryManager, Task, string> job, WorkingDirectoryManager wdb, Task task)
        {
            // 超厨二病だけどまぁやってることは外れてはいない気がするのでよしとする.
            string result;
            try
            {
                result = job(wdb, task);
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return result;
        }
        
        public string AssignBuiltin(WorkingDirectoryManager _workingDirectoryManager, Task task)
        {
            switch (task.Command.Fn)
            {
                case "ls":
                {
                    return $"{DoInSandBox(Ls, _workingDirectoryManager, task)}\n";
                }
                case "pwd":
                {
                    return $"{DoInSandBox(Pwd, _workingDirectoryManager, task)}\n";
                }
                case "whoami":
                {
                    return $"{WhoAmI()}\n";
                }
                case "cd":
                {
                    // 何も表示しないので改行はつけない
                    return $"{DoInSandBox(Cd, _workingDirectoryManager, task)}";
                }
                case "clear":
                {
                    // 何も表示しないので改行はつけない
                    return $"{DoInSandBox(Clear, _workingDirectoryManager, task)}";
                }
            }
            return $"error: `{task.Command.Fn}`\nmsg: this built-in function is not assigned yet.\n";
        }
        
        // --------------------------------------------------------------------------------------
        
        public static string WhoAmI()
        {
            var whoami = new WhoAmI();
            return whoami.GetUserName();
        }
        
        // --------------------------------------------------------------------------------------

        public string Clear(WorkingDirectoryManager wdm, Task task)
        {
            Console.Clear();
            return "";
        }
        
        public string Ls(WorkingDirectoryManager wdm, Task task)
        {
            
            // sortが必要そう。
            // なんか他のだと普通、横に並べてる。
            string _args;
            
            // 期待するもの
            // List<string> args, args.Count() => 1
            
            // 引数なしの場合
            if (!task.Command.Args.Any())
                _args = "";
            // 引数多すぎ問題
            else if (task.Command.Args.Count > 1)
                throw new Exception("too many arguments");
            else
                _args = task.Command.Args[0];

            var ls = new Ls();
            return ls.GetFileAndDirectoryNameOnlyPrint(wdm.Get(), _args, task.Command.Ops);
        }
        
        public string Cd(WorkingDirectoryManager wdm, Task task)
        {
            // todo : 引数チェック関数の導入
            string _args;
            if (!task.Command.Args.Any())
                _args = "";
            // 引数多すぎ問題
            else if (task.Command.Args.Count > 1)
                throw new Exception("too many arguments");
            _args = task.Command.Args[0];
            
            var cd = new Cd();
            // WhereAmI = cd.Move(WhereAmI, path);// 時間かかる。

            // Console.WriteLine($"[INFO CD] old path: {wdm.Get()}");
            var newPath = cd.MoveQuick(wdm.Get(), _args);
            wdm.Set(newPath);
            // Console.WriteLine($"[INFO CD] new path: {newPath}");
            // WhereAmI = cd.MoveQuick(wdm.Get(), _args);
            return "";
        }
        
        public string Pwd(WorkingDirectoryManager wdm, Task task)
        {
            return wdm.Get();
        }

        public string Cat(string path)
        {
            return "";
        }
    }
}