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

        public Repository(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            this.git = new Git(new FileRepository(Path.Combine(path, ".git")));
        }

        public IEnumerable<Commit> Log()
        {
            return this.git
                .Log()
                .Call()
                .Select(commit => new Commit(commit.GetFullMessage(), commit.GetShortMessage()));
        }
    }
}