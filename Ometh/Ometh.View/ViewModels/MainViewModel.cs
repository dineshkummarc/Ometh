using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Ometh.Core;
using Rareform.Patterns.MVVM;
using Rareform.Reflection;

namespace Ometh.View.ViewModels
{
    internal class MainViewModel : ViewModelBase<MainViewModel>, IDataErrorInfo
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
                        this.currentRepository.Load();

                        this.Commits = this.currentRepository.Commits
                            .Select(commit => new CommitViewModel(commit));
                    },
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

        private bool HasErrors(params string[] propertyName)
        {
            return propertyName.Any(property => !String.IsNullOrEmpty(this[property]));
        }
    }
}