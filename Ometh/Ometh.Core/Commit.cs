using System;

namespace Ometh.Core
{
    public class Commit
    {
        public string ShortMessage { get; private set; }

        public string FullMessage { get; private set; }

        public string Author { get; private set; }

        public DateTime CommitTime { get; private set; }

        public Commit(string fullMessage, string shortMessage, string author, DateTime commitTime)
        {
            this.FullMessage = fullMessage;
            this.ShortMessage = shortMessage;
            this.Author = author;
            this.CommitTime = commitTime;
        }
    }
}