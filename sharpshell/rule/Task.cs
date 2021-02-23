namespace sharpshell.rule
{
    public struct Task
    {
        // 存在意義不明?
        public readonly TaskType TaskType;
        public readonly Command Command;
        public readonly string BinPath;

        public Task(TaskType type, Command cmd, string binPath="")
        {
            TaskType = type;
            Command = cmd;
            BinPath = binPath;
        }
    }
}