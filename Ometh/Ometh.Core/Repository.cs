using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static bool IsValidRepository(string path)
        {
            // TODO: Check if the .git directory is actually a repository
            return Directory.Exists(Path.Combine(path, ".git"));
        }

        public string GetFullMessage(string hash)
        {
            var enumerable = this.git
                .Log()
                .Call();

            return enumerable.First(commit => commit.Name == hash).GetFullMessage();
        }

        public void Load()
        {
            this.LoadCommits();
            this.LoadRefs();
        }

        private void LoadCommits()
        {
            var enumerable = this.git
                .Log()
                .Call()
                .Select(this.ToCommit);

            foreach (Commit commit in enumerable)
            {
                this.commits.Add(commit.Hash, commit);
            }
        }

        private void LoadRefs()
        {
            var tags = this.git
                .GetRepository()
                .GetAllRefs()
                .Where(reference => reference.Key != "HEAD")
                .ToDictionary(entry => entry.Key, entry => entry.Value.GetObjectId().Name);

            foreach (KeyValuePair<string, string> pair in tags)
            {
                if (this.commits.ContainsKey(pair.Value))
                {
                    this.commits[pair.Value].AddReference(new Reference(pair.Key));
                }

                else
                {
                    Debug.WriteLine("Could not find commit " + pair.Value);
                }
            }
        }

        private Commit ToCommit(RevCommit commit)
        {
            return new Commit
            (
                this,
                commit.Name,
                commit.GetShortMessage(),
                commit.GetAuthorIdent().GetName(),
                commit.GetCommitterIdent().GetWhen(),
                commit.Parents.Select(p => p.Name)
            );
        }
    }
}