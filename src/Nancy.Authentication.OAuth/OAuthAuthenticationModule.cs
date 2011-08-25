namespace Nancy.Authentication.OAuth
{
    using ModelBinding;

    public class OAuthAuthenticationModule : NancyModule
    {
        public OAuthAuthenticationModule(
            IAuthenticationProvider authenticationProvider) : base(OAuth.Configuration.Base)
        {
            //this.RequiresAuthentication();

            Get[OAuth.Configuration.AuthenticationRoute] = parameters =>
            {
                var authentication =
                    this.Bind<Authentication>();

                var token = 
                    authenticationProvider.Authenticate(authentication);

                // Should handle invalid authentication

                return token;
            };
        }
    }
}