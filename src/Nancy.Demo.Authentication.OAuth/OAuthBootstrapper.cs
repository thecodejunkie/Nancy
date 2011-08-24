namespace Nancy.Demo.Authentication.OAuth
{
    using System;
    using System.Dynamic;
    using Extensions;
    using Nancy.Authentication.Forms;
    using Nancy.Authentication.OAuth;

    public class OAuthBootstrapper : DefaultNancyBootstrapper
    {
        protected override void InitialiseInternal(TinyIoC.TinyIoCContainer container)
        {
            base.InitialiseInternal(container);

            OAuth.Enable(this, new OAuthConfiguration
            {
                Authorization = "/authorize",
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

    public class RootModule : NancyModule
    {
        public RootModule()
        {
            Get["/"] = parameters =>
            {
                return "This is the root";
            };

            Get["/login"] = x =>
            {
                dynamic model = new ExpandoObject();
                model.Errored = this.Request.Query.error.HasValue;

                return View["login", model];
            };

            Post["/login"] = x =>
            {
                var userGuid = UserDatabase.ValidateUser((string)this.Request.Form.Username, (string)this.Request.Form.Password);

                if (userGuid == null)
                {
                    return Context.GetRedirect("~/login?error=true&username=" + (string)this.Request.Form.Username);
                }

                DateTime? expiry = null;
                if (this.Request.Form.RememberMe.HasValue)
                {
                    expiry = DateTime.Now.AddDays(7);
                }

                return this.LoginAndRedirect(userGuid.Value, expiry);
            };
        }
    }
}