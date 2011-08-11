namespace Nancy.Demo.Authentication.OAuth
{
    using Nancy.Authentication.OAuth;

    public class OAuthBootstrapper : DefaultNancyBootstrapper
    {
        protected override void InitialiseInternal(TinyIoC.TinyIoCContainer container)
        {
            OAuth.Enable(this, new OAuthConfiguration
            {
                Authorization = "/authorize",
                Base = "/oauth"
            });

            base.InitialiseInternal(container);
        }
    }

    public class RootModule : NancyModule
    {
        public RootModule()
        {
            Get["/"] = parameters => {
                return "This is the root";
            };
        }
    }
}