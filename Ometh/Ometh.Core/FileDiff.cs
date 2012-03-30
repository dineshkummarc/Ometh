namespace Ometh.Core
{
    public class FileDiff
    {
        public string Path { get; private set; }

        public FileDiff(string path)
        {
            this.Path = path;
        }
    }
}