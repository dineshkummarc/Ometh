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

        public CommitViewModel(Commit commit)
        {
            this.commit = commit;
        }
    }
}