using System;
using System.Collections.Generic;
using System.Linq;

namespace Ometh.Core
{
    public class Commit : IEquatable<Commit>
    {
        private readonly HashSet<string> parents;
        private readonly List<Reference> references;
        private readonly Repository repository;

        public string ShortMessage { get; private set; }

        public Person Author { get; private set; }

        public Person Committer { get; private set; }

        public DateTime CommitTime { get; private set; }

        public string Hash { get; private set; }

        public IEnumerable<string> Parents
        {
            get { return this.parents; }
        }

        public IEnumerable<Reference> References
        {
            get { return this.references; }
        }

        public Commit(Repository repository, string hash, string shortMessage, Person author, Person commiter, DateTime commitTime, IEnumerable<string> parents)
        {
            this.repository = repository;
            this.Hash = hash;
            this.ShortMessage = shortMessage;
            this.Author = author;
            this.Committer = commiter;
            this.CommitTime = commitTime;

            this.references = new List<Reference>();

            this.parents = new HashSet<string>();

            foreach (string commit in parents)
            {
                this.parents.Add(commit);
            }
        }

        public string GetFullMessage()
        {
            return this.repository.GetFullMessage(this.Hash);
        }

        public IEnumerable<FileDiff> GetFileDiffs()
        {
            return this.parents.Any()
                       ? this.repository.GetFileDiffs(this.Hash, this.parents.First())
                       : Enumerable.Empty<FileDiff>();
        }

        public void AddReference(Reference reference)
        {
            this.references.Add(reference);
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