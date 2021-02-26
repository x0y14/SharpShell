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

        private string DoInSandBox(Func<VirtualPathManager, ShellTask, string> job, VirtualPathManager wdb, ShellTask Shelltask)
        {
            // 超厨二病だけどまぁやってることは外れてはいない気がするのでよしとする.
            string result;
            try
            {
                result = job(wdb, Shelltask);
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return result;
        }
        
        public string AssignBuiltin(VirtualPathManager _virtualPathManager, ShellTask Shelltask)
        {
            switch (Shelltask.Command.Fn)
            {
                case "ls":
                {
                    return $"{DoInSandBox(Ls, _virtualPathManager, Shelltask)}\n";
                }
                case "pwd":
                {
                    return $"{DoInSandBox(Pwd, _virtualPathManager, Shelltask)}\n";
                }
                case "whoami":
                {
                    return $"{WhoAmI()}\n";
                }
                case "cd":
                {
                    // 何も表示しないので改行はつけない
                    return $"{DoInSandBox(Cd, _virtualPathManager, Shelltask)}";
                }
                case "clear":
                {
                    // 何も表示しないので改行はつけない
                    return $"{DoInSandBox(Clear, _virtualPathManager, Shelltask)}";
                }
            }
            return $"error: `{Shelltask.Command.Fn}`\nmsg: this built-in function is not assigned yet.\n";
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

        public string Clear(VirtualPathManager vpm, ShellTask Shelltask)
        {
            Console.Clear();
            return "";
        }
        
        public string Ls(VirtualPathManager vpm, ShellTask Shelltask)
        {
            
            if (Shelltask.Command.Args.Count > 1)
                throw new Exception("too many arguments");

            var _args = GetFirstArgument(Shelltask.Command.Args);
            
            var ls = new Ls();
            return ls.GetFileAndDirectoryNameOnlyPrint(vpm.GetWorkingDirectoryAsString(), _args, Shelltask.Command.Ops);
        }
        
        public string Cd(VirtualPathManager vpm, ShellTask Shelltask)
        {
            // ここはあくまで、引数、オプションチェックの場所。
            if (Shelltask.Command.Args.Count > 1)
                throw new Exception("too many arguments");
            var _args = GetFirstArgument(Shelltask.Command.Args);
            
            var cd = new Cd(vpm);
            cd.MoveQuick(_args);// オプションなしの場合の動き。
            
            // vpm.SetAbsolutePath(newPath);// これ消したい。ここから叩くようなものではない
            return "";
        }
        
        public string Pwd(VirtualPathManager vpm, ShellTask Shelltask)
        {
            return vpm.GetWorkingDirectoryAsString();
        }

        public string Cat(string path)
        {
            return "";
        }
    }
}