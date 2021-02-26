namespace sharpshell.rule
{
    public struct ShellTask
    {
        // 存在意義不明?
        public readonly ShellTaskType TaskType;
        public readonly Command Command;
        public readonly string BinPath;
        public readonly string RawInput;

        public ShellTask(ShellTaskType type, string raw, Command cmd, string binPath="")
        {
            TaskType = type;
            RawInput = raw;
            Command = cmd;
            BinPath = binPath;
        }
    }
}