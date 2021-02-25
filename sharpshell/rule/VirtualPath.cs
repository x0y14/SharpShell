namespace sharpshell.rule
{
    public struct VirtualPath
    {
        public readonly VirtualPathType Type;
        public readonly string AbsolutePath;

        public VirtualPath(VirtualPathType type, string absolutePath)
        {
            Type = type;
            AbsolutePath = absolutePath;
        }
    }
}