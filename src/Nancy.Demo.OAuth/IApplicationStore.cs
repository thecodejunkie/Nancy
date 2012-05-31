namespace Nancy.Demo.OAuth
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface IApplicationStore : IEnumerable<Application>
    {
        void Add(Application application);
    }

    public class ApplicationStore : IApplicationStore
    {
        private readonly IList<Application> applications;

        public ApplicationStore(ApplicationPermissionManager permissionManager)
        {
            this.applications = new List<Application>
            {
                new Application(
                    new Guid("4D71889E-89D2-46DB-BC30-60428073B4AA"),
                    "Nancy Demo Application",
                    "Test application in the Demo app. This application is hard coded into the IApplicationStore and will not vanish when your application recycles",
                    new Uri("http://nancyfx.org"),
                    new Uri("http://nancyfx.org/callback"),
                    permissionManager.GetDefaultPermissions())
            };
        }

        public void Add(Application application)
        {
            this.applications.Add(application);
        }

        public IEnumerator<Application> GetEnumerator()
        {
            return this.applications.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}