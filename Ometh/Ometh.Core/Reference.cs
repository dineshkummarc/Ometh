using System;

namespace Ometh.Core
{
    public class Reference
    {
        public string Name { get; private set; }

        public bool IsHead { get; private set; }

        public bool IsRemote { get; private set; }

        public bool IsTag { get; private set; }

        public Reference(string fullName)
        {
            this.IsHead = fullName.StartsWith("refs/heads/");
            this.IsRemote = fullName.StartsWith("refs/remotes/");
            this.IsTag = fullName.StartsWith("refs/tags/");

            this.Name = !this.IsRemote
                            ? fullName.Substring(fullName.LastIndexOf('/') + 1)
                            : fullName.Replace("refs/remotes/", String.Empty);
        }
    }
}