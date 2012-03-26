using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NGit.Api;
using NGit.Revwalk;
using NGit.Storage.File;

namespace Ometh.Core
{
    public class Repository
    {
        private readonly Git git;
        private readonly Dictionary<string, Commit> commits;

        public IEnumerable<Commit> Commits
        {
            get { return this.commits.Values; }
        }

        public Repository(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            this.git = new Git(new FileRepository(Path.Combine(path, ".git")));
            this.commits = new Dictionary<string, Commit>();
        }

        public void Load()
        {
            this.LoadCommits();
            this.LoadTags();
        }

        private void LoadCommits()
        {
            var enumerable = this.git
                .Log()
                .Call()
                .Select(ToCommit);

            foreach (Commit commit in enumerable)
            {
                this.commits.Add(commit.Hash, commit);
            }
        }

        private void LoadTags()
        {
            var tags = this.git
                .GetRepository()
                .GetTags()
                .ToDictionary(entry => entry.Key, entry => entry.Value.GetObjectId().Name);

            foreach (KeyValuePair<string, string> pair in tags)
            {
                this.commits[pair.Value].Tag = pair.Key;
            }
        }

        private static Commit ToCommit(RevCommit commit)
        {
            return new Commit
            (
                commit.Name,
                commit.GetFullMessage(),
                commit.GetShortMessage(),
                commit.GetAuthorIdent().GetName(),
                commit.GetCommitterIdent().GetWhen(),
                commit.Parents.Select(p => p.Name)
            );
        }
    }
}