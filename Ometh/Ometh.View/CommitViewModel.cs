using System;
using Ometh.Core;
using Rareform.Patterns.MVVM;

namespace Ometh.View
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

        public CommitViewModel(Commit commit)
        {
            this.commit = commit;
        }
    }
}