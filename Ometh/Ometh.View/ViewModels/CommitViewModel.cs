﻿using System;
using System.Collections.Generic;
using System.Linq;
using Ometh.Core;
using Rareform.Patterns.MVVM;

namespace Ometh.View.ViewModels
{
    internal class CommitViewModel : ViewModelBase<CommitViewModel>
    {
        private readonly Commit commit;

        public string Hash
        {
            get { return this.commit.Hash; }
        }

        public string ShortMessage
        {
            get { return this.commit.ShortMessage; }
        }

        public string FullMessage
        {
            get { return this.commit.GetFullMessage(); }
        }

        public string AuthorName
        {
            get { return this.commit.Author.Name; }
        }

        public string AuthorEmail
        {
            get { return this.commit.Author.Email; }
        }

        public string CommitterName
        {
            get { return this.commit.Committer.Name; }
        }

        public string CommiterAuthor
        {
            get { return this.commit.Committer.Email; }
        }

        public DateTime CommitTime
        {
            get { return this.commit.CommitTime.ToLocalTime(); }
        }

        public IEnumerable<ReferenceViewModel> References
        {
            get
            {
                var refs = this.commit.References
                    .OrderByDescending(reference => reference.IsTag)
                    .ThenByDescending(reference => reference.IsHead)
                    .ThenByDescending(reference => reference.IsRemote)
                    .Select(reference => new ReferenceViewModel(reference));

                return refs;
            }
        }

        public bool HasReferences
        {
            get { return this.References.Any(); }
        }

        public CommitViewModel(Commit commit)
        {
            this.commit = commit;
        }
    }
}