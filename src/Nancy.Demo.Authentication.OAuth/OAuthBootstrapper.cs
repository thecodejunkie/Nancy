namespace Nancy.Demo.Authentication.OAuth
{
    using Nancy.Authentication.Forms;
    using Nancy.Authentication.OAuth;

    public class OAuthBootstrapper : DefaultNancyBootstrapper
    {
        protected override void InitialiseInternal(TinyIoC.TinyIoCContainer container)
        {
            base.InitialiseInternal(container);

            OAuth.Enable(this, new OAuthConfiguration
            {
                AuthorizationRequestRoute = "/authorize",
                AuthorizationAllowRoute = "/allow",
                AuthorizationDenyRoute = "/deny",
                AuthorizationView = "authorize",
                AuthenticationRoute = "/access_token",
                Base = "/oauth"
            });

            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration()
                {
                    RedirectUrl = "~/login",
                    UsernameMapper = container.Resolve<IUsernameMapper>(),
                };

            FormsAuthentication.Enable(this, formsAuthConfiguration);
        }
    }
}