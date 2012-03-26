using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Ometh.Core;
using Rareform.Patterns.MVVM;

namespace Ometh.View
{
    internal class MainViewModel : ViewModelBase<MainViewModel>
    {
        private IEnumerable<CommitViewModel> commits;
        private string repositoryPath;
        private Repository currentRepository;

        public IEnumerable<CommitViewModel> Commits
        {
            get { return this.commits; }
            private set
            {
                this.commits = value;
                this.OnPropertyChanged(vm => vm.Commits);
            }
        }

        public string RepositoryPath
        {
            get { return this.repositoryPath; }
            set
            {
                this.repositoryPath = value;
                this.OnPropertyChanged(vm => vm.RepositoryPath);
            }
        }

        public ICommand OpenRepositoryCommand
        {
            get
            {
                return new RelayCommand
                (
                    param =>
                    {
                        this.currentRepository = new Repository(this.RepositoryPath);

                        this.Commits = this.currentRepository.Log()
                            .Select(commit => new CommitViewModel(commit));
                    }
                );
            }
        }
    }
}