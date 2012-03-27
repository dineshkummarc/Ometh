﻿using System;
using System.Collections.Generic;

namespace Ometh.Core
{
    public class Commit : IEquatable<Commit>
    {
        private readonly HashSet<string> parents;
        private readonly List<Reference> references;

        public string ShortMessage { get; private set; }

        public string FullMessage { get; private set; }

        public string Author { get; private set; }

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

        public Commit(string hash, string fullMessage, string shortMessage, string author, DateTime commitTime, IEnumerable<string> parents)
        {
            this.Hash = hash;
            this.FullMessage = fullMessage;
            this.ShortMessage = shortMessage;
            this.Author = author;
            this.CommitTime = commitTime;

            this.references = new List<Reference>();

            this.parents = new HashSet<string>();

            foreach (string commit in parents)
            {
                this.parents.Add(commit);
            }
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