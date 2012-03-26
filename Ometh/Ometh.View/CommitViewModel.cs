using Ometh.Core;
using Rareform.Patterns.MVVM;

namespace Ometh.View
{
    internal class CommitViewModel : ViewModelBase<CommitViewModel>
    {
        private readonly Commit commit;

        public string Message
        {
            get { return this.commit.Message; }
        }

        public CommitViewModel(Commit commit)
        {
            this.commit = commit;
        }
    }
}