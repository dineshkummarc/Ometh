namespace Ometh.Core
{
    public class Commit
    {
        public string ShortMessage { get; private set; }

        public string FullMessage { get; private set; }

        public Commit(string fullMessage, string shortMessage)
        {
            this.FullMessage = fullMessage;
            this.ShortMessage = shortMessage;
        }
    }
}