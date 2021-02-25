using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using sharpshell.rule;
using sharpshell.virtualpath;

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

        private string DoInSandBox(Func<VirtualPathManager, Task, string> job, VirtualPathManager wdb, Task task)
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
        
        public string AssignBuiltin(VirtualPathManager _virtualPathManager, Task task)
        {
            switch (task.Command.Fn)
            {
                case "ls":
                {
                    return $"{DoInSandBox(Ls, _virtualPathManager, task)}\n";
                }
                case "pwd":
                {
                    return $"{DoInSandBox(Pwd, _virtualPathManager, task)}\n";
                }
                case "whoami":
                {
                    return $"{WhoAmI()}\n";
                }
                case "cd":
                {
                    // 何も表示しないので改行はつけない
                    return $"{DoInSandBox(Cd, _virtualPathManager, task)}";
                }
                case "clear":
                {
                    // 何も表示しないので改行はつけない
                    return $"{DoInSandBox(Clear, _virtualPathManager, task)}";
                }
            }
            return $"error: `{task.Command.Fn}`\nmsg: this built-in function is not assigned yet.\n";
        }

        public string GetFirstArgument(List<string> args)
        {
            if (!args.Any())
                return "";
            return args[0];
        }
        
        // --------------------------------------------------------------------------------------
        
        public static string WhoAmI()
        {
            var whoami = new WhoAmI();
            return whoami.GetUserName();
        }
        
        // --------------------------------------------------------------------------------------

        public string Clear(VirtualPathManager vpm, Task task)
        {
            Console.Clear();
            return "";
        }
        
        public string Ls(VirtualPathManager vpm, Task task)
        {
            
            if (task.Command.Args.Count > 1)
                throw new Exception("too many arguments");

            var _args = GetFirstArgument(task.Command.Args);
            
            var ls = new Ls();
            return ls.GetFileAndDirectoryNameOnlyPrint(vpm.GetWorkingDirectoryAsString(), _args, task.Command.Ops);
        }
        
        public string Cd(VirtualPathManager vpm, Task task)
        {
            // ここはあくまで、引数、オプションチェックの場所。
            if (task.Command.Args.Count > 1)
                throw new Exception("too many arguments");
            var _args = GetFirstArgument(task.Command.Args);
            
            var cd = new Cd(vpm);
            cd.MoveQuick(_args);// オプションなしの場合の動き。
            
            // vpm.SetAbsolutePath(newPath);// これ消したい。ここから叩くようなものではない
            return "";
        }
        
        public string Pwd(VirtualPathManager vpm, Task task)
        {
            return vpm.GetWorkingDirectoryAsString();
        }

        public string Cat(string path)
        {
            return "";
        }
    }
}