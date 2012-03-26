namespace Ometh.Core
{
    public class Commit
    {
        public string Message { get; private set; }

        public Commit(string message)
        {
            this.Message = message;
        }
    }
}