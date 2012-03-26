using System;
using System.Collections.Generic;

namespace Ometh.Core
{
    public class Commit : IEquatable<Commit>
    {
        private readonly HashSet<string> parents;

        public string ShortMessage { get; private set; }

        public string FullMessage { get; private set; }

        public string Author { get; private set; }

        public DateTime CommitTime { get; private set; }

        public string Hash { get; private set; }

        public IEnumerable<string> Parents
        {
            get { return this.parents; }
        }

        public Commit(string hash, string fullMessage, string shortMessage, string author, DateTime commitTime, IEnumerable<string> parents)
        {
            this.Hash = hash;
            this.FullMessage = fullMessage;
            this.ShortMessage = shortMessage;
            this.Author = author;
            this.CommitTime = commitTime;

            this.parents = new HashSet<string>();

            foreach (string commit in parents)
            {
                this.parents.Add(commit);
            }
        }

        public bool Equals(Commit other)
        {
            return other != null && this.Hash == other.Hash;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Commit);
        }

        public override int GetHashCode()
        {
            return new { this.Hash }.GetHashCode();
        }
    }
}