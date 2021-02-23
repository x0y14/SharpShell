using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using sharpshell.rule;

namespace sharpshell.mod
{
    public class Assignor
    {
        /// <summary>
        /// Q. 何をするもの?
        /// A. 解析されたコマンドを種類ごとに分別し、Shellに返す。
        ///
        /// Q. なんのためにあるの？
        /// A. タスクマネージャーとユーザ入力の橋渡しのためにあります。ここで生成したデータをもとにタスクマネージャーによってコマンドが実行されます。
        ///    (タスクマネージャーじゃなくてプロセスマネージャーで良くね？)
        ///
        /// (このsummaryって何。)
        /// 
        /// shell(#input) -> listener -> parser -> listener -> shell
        /// shell -> assignor -> shell
        /// shell -> processManager -> shell(#output)
        /// こんなになる予定
        /// なんか複雑だけどこれでいいのかな。
        /// 行ったり来たりしれる。
        ///
        /// コマンドの割り当て優先度は
        /// 1, 組み込み
        /// 2, ショートカットパス。まぁいわゆるパス。
        /// 3, 現在のパス
        ///
        /// で良いかな？
        /// 
        /// </summary>

        private string WhereAmI;
        private List<string> ShortCutPath;
        private List<string> BuiltinCmd;
        public Assignor(string whereami, List<string> path, List<string> builtin)
        {
            WhereAmI = whereami;
            ShortCutPath = path;
            BuiltinCmd = builtin;
        }

        private bool SearchBin(string path, string bin)
        {
            return Directory.GetFiles(path).Contains(bin);
        }

        public Task Assign(Command cmd)
        {
            // 1. 組み込み
            if (BuiltinCmd.Contains(cmd.Fn))
                return new Task(TaskType.BUILTIN, cmd);

            // 2. パス
            foreach (var shortcutPath in ShortCutPath)
            {
                if (SearchBin(shortcutPath, cmd.Fn))
                    return new Task(TaskType.UNKNOWN, cmd, $"{shortcutPath}{cmd.Fn}");
            }
            
            // 3. 現在のパス
            if (SearchBin(WhereAmI, cmd.Fn))
                return new Task(TaskType.UNKNOWN, cmd, $"{WhereAmI}{cmd.Fn}");
            
            return new Task(TaskType.NOTFOUND, cmd);

        }
        
    }
}