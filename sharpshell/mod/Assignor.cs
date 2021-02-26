using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using sharpshell.rule;
using sharpshell.builtin;
using sharpshell.virtualpath;

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

        private readonly List<string> ShortCutPath;

        public Assignor(List<string> paths)
        {
            ShortCutPath = paths;
        }

        private bool SearchBin(string path, string bin)
        {
            return Directory.GetFiles(path).Contains(bin);
        }

        public ShellTask Assign(VirtualPathManager _virtualPathManager, string raw, Command cmd)
        {
            // 改行だけの場合はNOOP(何もしない)を返す.
            if (cmd.Fn.Equals("") || cmd.Fn.Equals("\t") || cmd.Fn.Equals("\n"))
                return new ShellTask(ShellTaskType.NOOP, raw, cmd);
            
            // 1. 組み込み
            if (BuiltinManager.IsSupporting(cmd.Fn))
                return new ShellTask(ShellTaskType.BUILTIN, raw, cmd);

            // 2. パス
            foreach (var shortcutPath in ShortCutPath)
            {
                var unknown = VirtualPathManager.IsExist($"{shortcutPath}/{cmd.Fn}");
                if (unknown.Type == VirtualPathType.FILE)
                // if (SearchBin(shortcutPath, cmd.Fn))
                return new ShellTask(ShellTaskType.UNKNOWN, raw, cmd, unknown.AbsolutePath);
            }

            // 3. 実行ファイル
            var virtualPath = _virtualPathManager.AnalyzePath(cmd.Fn);
            if (virtualPath.Type == VirtualPathType.FILE)
                return new ShellTask(ShellTaskType.UNKNOWN, raw, cmd, virtualPath.AbsolutePath);
            
            return new ShellTask(ShellTaskType.NOTFOUND, raw, cmd);

        }
        
    }
}