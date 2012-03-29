using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Ometh.Core;
using Rareform.Patterns.MVVM;
using Rareform.Reflection;

namespace Ometh.View.ViewModels
{
    internal class MainViewModel : ViewModelBase<MainViewModel>, IDataErrorInfo
    {
        private string repositoryPath;
        private Repository currentRepository;
        private CommitViewModel selectedCommit;
        private bool isLoadingRepository;

        public IEnumerable<CommitViewModel> Commits
        {
            get
            {
                return this.currentRepository == null || this.currentRepository.Commits == null
                           ? null
                           : this.currentRepository.Commits.Select(commit => new CommitViewModel(commit));
            }
        }

        public CommitViewModel SelectedCommit
        {
            get { return this.selectedCommit; }
            set
            {
                this.selectedCommit = value;
                this.OnPropertyChanged(vm => vm.SelectedCommit);
            }
        }

        public int CommitCount
        {
            get { return this.Commits.Count(); }
        }

        public bool HasCommits
        {
            get { return this.Commits != null && this.Commits.Any(); }
        }

        public bool IsLoadingRepository
        {
            get { return this.isLoadingRepository; }
            private set
            {
                this.isLoadingRepository = value;
                this.OnPropertyChanged(vm => vm.IsLoadingRepository);
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

                    param => Task.Factory.StartNew(() =>
                    {
                        this.currentRepository = new Repository(this.RepositoryPath);

                        this.IsLoadingRepository = true;
                        this.SelectedCommit = null;

                        this.currentRepository.Load();

                        this.IsLoadingRepository = false;

                        this.OnPropertyChanged(vm => vm.Commits);
                        this.OnPropertyChanged(vm => vm.CommitCount);
                        this.OnPropertyChanged(vm => vm.HasCommits);
                    }),
                    param => !this.HasErrors(Reflector.GetMemberName(() => this.RepositoryPath))
                );
            }
        }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <returns>The error message for the property. The default is an empty string ("").</returns>
        public string this[string columnName]
        {
            get
            {
                if (columnName == Reflector.GetMemberName(() => this.RepositoryPath))
                {
                    if (String.IsNullOrWhiteSpace(this.RepositoryPath))
                    {
                        return "Specify a directory.";
                    }

                    if (!Directory.Exists(this.RepositoryPath))
                    {
                        return "Directory doesn't exist.";
                    }

                    if (!Repository.IsValidRepository(this.RepositoryPath))
                    {
                        return "Not a valid git repository.";
                    }
                }

                return String.Empty;
            }
        }

        public string Error
        {
            get { return null; }
        }

        public MainViewModel()
        {
#if DEBUG
            // If we are debugging, load the own repository
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                this.RepositoryPath =
                    new DirectoryInfo(Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.Parent.Parent.
                        FullName;

                this.OpenRepositoryCommand.Execute(null);
            }
#endif
        }

        private bool HasErrors(params string[] propertyName)
        {
            return propertyName.Any(property => !String.IsNullOrEmpty(this[property]));
        }
    }
}