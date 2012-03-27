using Ometh.Core;

namespace Ometh.View.ViewModels
{
    public class ReferenceViewModel
    {
        private readonly Reference reference;

        public string Name
        {
            get { return this.reference.Name; }
        }

        public bool IsTag
        {
            get { return this.reference.IsTag; }
        }

        public bool IsRemote
        {
            get { return this.reference.IsRemote; }
        }

        public bool IsHead
        {
            get { return this.reference.IsHead; }
        }

        public ReferenceViewModel(Reference reference)
        {
            this.reference = reference;
        }
    }
}