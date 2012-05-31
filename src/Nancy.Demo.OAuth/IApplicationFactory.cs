namespace Nancy.Demo.OAuth
{
    public interface IApplicationFactory
    {
        Application Create(ApplicationModel model);
    }

    public class DefaultApplicationFactory : IApplicationFactory
    {
        private readonly ApplicationPermissionManager permissionManager;

        public DefaultApplicationFactory(ApplicationPermissionManager permissionManager)
        {
            this.permissionManager = permissionManager;
        }

        public Application Create(ApplicationModel model)
        {
            return new Application(model, permissionManager.GetDefaultPermissions());
        }
    }
}