namespace Nancy.Demo.Authentication.OAuth
{
    using Nancy.Authentication.Forms;
    using Nancy.Authentication.OAuth;

    public class OAuthBootstrapper : DefaultNancyBootstrapper
    {
        protected override void InitialiseInternal(TinyIoC.TinyIoCContainer container)
        {
            base.InitialiseInternal(container);

            //OAuth.Enable(this, new OAuthConfiguration
            //{
            //    AuthorizationRequestRoute = "/authorize",
            //    AuthorizationAllowRoute = "/allow",
            //    AuthorizationDenyRoute = "/deny",
            //    AuthorizationView = "authorize",
            //    AuthenticationRoute = "/access_token",
            //    Base = "/oauth"
            //});

            //OAuth.Enable(this, with =>{
            //    with.AuthorizationRequestRoute = "/authorize";
            //    with.AuthorizationAllowRoute = "/allow";
            //    with.AuthorizationDenyRoute = "/deny";
            //    with.AuthorizationView = "authorize";
            //    with.AuthenticationRoute = "/access_token";
            //    with.Base = "/oauth";
            //});

            OAuth.Enable(this);

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