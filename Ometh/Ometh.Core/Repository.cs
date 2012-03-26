using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NGit.Api;
using NGit.Storage.File;

namespace Ometh.Core
{
    public class Repository
    {
        private readonly Git git;
        private readonly HashSet<Commit> commits;

        public IEnumerable<Commit> Commits
        {
            get { return this.commits; }
        }

        public Repository(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            this.git = new Git(new FileRepository(Path.Combine(path, ".git")));
            this.commits = new HashSet<Commit>();
        }

        public void Load()
        {
            this.LoadCommits();
        }

        private void LoadCommits()
        {
            var enumerable = this.git
                .Log()
                .Call()
                .Select(commit =>
                    new Commit
                    (
                        commit.Name,
                        commit.GetFullMessage(),
                        commit.GetShortMessage(),
                        commit.GetAuthorIdent().GetName(),
                        commit.GetCommitterIdent().GetWhen()
                    ));

            foreach (Commit commit in enumerable)
            {
                this.commits.Add(commit);
            }
        }
    }
}