namespace Ometh.Core
{
    public class FileDiff
    {
        public string Path { get; private set; }

        public DiffType DiffType { get; private set; }

        public FileDiff(string path, DiffType diffType)
        {
            this.Path = path;
            this.DiffType = diffType;
        }
    }
}