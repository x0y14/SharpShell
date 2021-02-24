namespace sharpshell.builtin
{
    public class Cat
    {
        public Cat(){}

        public string GetData(string path)
        {
            return System.IO.File.ReadAllText($@"{path}");
        }
    }
}