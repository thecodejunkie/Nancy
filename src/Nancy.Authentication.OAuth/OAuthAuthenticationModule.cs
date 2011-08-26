namespace Nancy.Authentication.OAuth
{
    using ModelBinding;

    public class OAuthAuthenticationModule : NancyModule
    {
        public OAuthAuthenticationModule(
            IAuthenticationProvider authenticationProvider,
            IAccessTokenGenerator accessTokenGenerator) : base(OAuth.Configuration.Base)
        {
            //this.RequiresAuthentication();

            Get[OAuth.Configuration.AuthenticationRoute] = parameters =>
            {
                var authentication =
                    this.Bind<Authentication>();

                var isAuthenticated = 
                    authenticationProvider.Authenticate(authentication);

                // Should handle invalid authentication

                var oauth_token =
                    accessTokenGenerator.Generate();

                var target =
                    string.Concat(authentication.Redirect_Uri, "?oauth_token=", oauth_token);

                return Response.AsRedirect(target);
            };
        }
    }
}