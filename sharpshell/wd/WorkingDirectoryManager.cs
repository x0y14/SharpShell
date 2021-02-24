namespace sharpshell.wd
{
    public class WorkingDirectoryManager
    {
        private string NowPath;

        public WorkingDirectoryManager(string now)
        {
            NowPath = now;
        }

        public string Get()
        {
            return NowPath;
        }

        public string Set(string newPath)
        {
            NowPath = newPath;
            return NowPath;
        }

    }
}