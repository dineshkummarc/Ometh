using System;
using System.Collections.Generic;
using System.Linq;

namespace Ometh.Core
{
    public class Commit : IEquatable<Commit>
    {
        private readonly HashSet<string> parents;
        private List<string> tags;

        public string ShortMessage { get; private set; }

        public string FullMessage { get; private set; }

        public string Author { get; private set; }

        public DateTime CommitTime { get; private set; }

        public string Hash { get; private set; }

        public IEnumerable<string> Parents
        {
            get { return this.parents; }
        }

        public IEnumerable<string> Tags
        {
            get { return this.tags; }
        }

        public bool IsTagged
        {
            get { return this.tags.Any(); }
        }

        public Commit(string hash, string fullMessage, string shortMessage, string author, DateTime commitTime, IEnumerable<string> parents)
        {
            this.Hash = hash;
            this.FullMessage = fullMessage;
            this.ShortMessage = shortMessage;
            this.Author = author;
            this.CommitTime = commitTime;

            this.tags = new List<string>();

            this.parents = new HashSet<string>();

            foreach (string commit in parents)
            {
                this.parents.Add(commit);
            }
        }

        public void AddTag(string tag)
        {
            this.tags.Add(tag);
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