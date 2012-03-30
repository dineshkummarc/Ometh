using Ometh.Core;

namespace Ometh.View.ViewModels
{
    public class FileDiffViewModel
    {
        private readonly FileDiff fileDiff;

        public string Path
        {
            get { return this.fileDiff.Path; }
        }

        public FileDiffViewModel(FileDiff fileDiff)
        {
            this.fileDiff = fileDiff;
        }
    }
}