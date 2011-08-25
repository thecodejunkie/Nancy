namespace Nancy.Authentication.OAuth
{
    /// <summary>
    /// Responsible for creating the model that should be passed to the view that gives the
    /// client the alternative to Allow or Deny access to the account.
    /// </summary>
    public interface IAuthorizationViewModelFactory
    {
        dynamic CreateViewModel(Authorization authorization, Application application);
    }

    public class DefaultAuthorizationViewModelFactory : IAuthorizationViewModelFactory
    {
        public dynamic CreateViewModel(Authorization authorization, Application application)
        {
            return application;
        }
    }
}