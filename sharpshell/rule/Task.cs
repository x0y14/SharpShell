namespace sharpshell.rule
{
    public struct Task
    {
        // 存在意義不明?
        public readonly TaskType TaskType;
        public readonly Command Command;
        public readonly string BinPath;
        public readonly string RawInput;

        public Task(TaskType type, string raw, Command cmd, string binPath="")
        {
            TaskType = type;
            RawInput = raw;
            Command = cmd;
            BinPath = binPath;
        }
    }
}