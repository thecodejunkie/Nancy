namespace Nancy.Demo.OAuth
{
    using Authentication.Forms;
    using Nancy.Bootstrapper;
    using Nancy.OAuth;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoC.TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration()
                {
                    RedirectUrl = "~/login",
                    UserMapper = container.Resolve<IUserMapper>(),
                };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);

            OAuthAuthentication.Enable(
                pipelines, 
                container.Resolve<IOAuthLogin>()
            );

            InMemorySessions.Enable(pipelines);
        }
    }
}