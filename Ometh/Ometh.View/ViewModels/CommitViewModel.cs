using System;
using System.Collections.Generic;
using Ometh.Core;
using Rareform.Patterns.MVVM;

namespace Ometh.View.ViewModels
{
    internal class CommitViewModel : ViewModelBase<CommitViewModel>
    {
        private readonly Commit commit;

        public string ShortMessage
        {
            get { return this.commit.ShortMessage; }
        }

        public string Author
        {
            get { return this.commit.Author; }
        }

        public DateTime CommitTime
        {
            get { return this.commit.CommitTime; }
        }

        public IEnumerable<string> Tags
        {
            get { return this.commit.Tags; }
        }

        public bool IsTagged
        {
            get { return this.commit.IsTagged; }
        }

        public CommitViewModel(Commit commit)
        {
            this.commit = commit;
        }
    }
}