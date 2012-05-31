namespace Nancy.Demo.OAuth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ApplicationPermissionManager
    {
        private readonly IList<Tuple<string, string, bool>> permissions;

        public ApplicationPermissionManager()
        {
            this.permissions = new List<Tuple<string, string, bool>>
            {
                new Tuple<string, string, bool>("Read", "Read the messages", true),
                new Tuple<string, string, bool>("Write", "Post new messages", false)
            };
        }

        public IEnumerable<string> GetAllPermissions()
        {
            return this.permissions.Select(x => x.Item1);
        }

        public IEnumerable<string> GetDefaultPermissions()
        {
            return this.permissions.Where(x => x.Item3).Select(x => x.Item1);
        }
    }
}