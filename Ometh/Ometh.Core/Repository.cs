using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NGit;
using NGit.Api;
using NGit.Diff;
using NGit.Revwalk;
using NGit.Storage.File;
using NGit.Treewalk;

namespace Ometh.Core
{
    public class Repository
    {
        private readonly Git git;
        private readonly List<Commit> commits;

        // Store the hashes as key for lookup when loading the refs
        private readonly Dictionary<string, Commit> commitLookup;

        public IEnumerable<Commit> Commits
        {
            get { return this.commits; }
        }

        public Repository(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            this.git = new Git(new FileRepository(Path.Combine(path, ".git")));
            this.commits = new List<Commit>();
            this.commitLookup = new Dictionary<string, Commit>();
        }

        public static bool IsValidRepository(string path)
        {
            // TODO: Check if the .git directory is actually a repository
            return Directory.Exists(Path.Combine(path, ".git"));
        }

        public string GetFullMessage(string hash)
        {
            var walk = new RevWalk(this.git.GetRepository());

            RevCommit commit = walk.ParseCommit(ObjectId.FromString(hash));

            walk.Dispose();

            return commit.GetFullMessage();
        }

        public IEnumerable<FileDiff> GetFileDiffs(string newHash, string oldHash)
        {
            NGit.Repository repo = this.git.GetRepository();

            ObjectId newCommit = repo.Resolve(newHash + "^{tree}");
            ObjectId oldCommit = repo.Resolve(oldHash + "^{tree}");

            var walk = new TreeWalk(repo) { Recursive = true };
            walk.AddTree(oldCommit);
            walk.AddTree(newCommit);

            IEnumerable<DiffEntry> entries = DiffEntry.Scan(walk);

            var diffs = entries.Where(diff => diff.GetNewId().Name != diff.GetOldId().Name);

            return from diffEntry in diffs
                   let diffType = ToDiffType(diffEntry.GetChangeType())
                   select new FileDiff(diffEntry.GetNewPath(), diffType);
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
                .Select(commit => ToCommit(commit, this));

            foreach (Commit commit in enumerable)
            {
                this.commits.Add(commit);
                this.commitLookup.Add(commit.Hash, commit);
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
                Commit commit;

                if (this.commitLookup.TryGetValue(pair.Value, out commit))
                {
                    commit.AddReference(new Reference(pair.Key));
                }

                else
                {
                    Debug.WriteLine("Could not find commit " + pair.Value);
                }
            }
        }

        private static Commit ToCommit(RevCommit commit, Repository repository)
        {
            return new Commit
            (
                repository,
                commit.Name,
                commit.GetShortMessage(),
                ToPerson(commit.GetAuthorIdent()),
                ToPerson(commit.GetCommitterIdent()),
                commit.GetCommitterIdent().GetWhen(),
                commit.Parents.Select(p => p.Name)
            );
        }

        private static Person ToPerson(PersonIdent ident)
        {
            return new Person(ident.GetName(), ident.GetEmailAddress());
        }

        private static DiffType ToDiffType(DiffEntry.ChangeType changeType)
        {
            switch (changeType)
            {
                case DiffEntry.ChangeType.MODIFY:
                    return DiffType.Modify;

                case DiffEntry.ChangeType.ADD:
                    return DiffType.Add;

                case DiffEntry.ChangeType.DELETE:
                    return DiffType.Delete;

                case DiffEntry.ChangeType.COPY:
                    return DiffType.Copy;

                case DiffEntry.ChangeType.RENAME:
                    return DiffType.Rename;
            }

            throw new InvalidOperationException();
        }
    }
}