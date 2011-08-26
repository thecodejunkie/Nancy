namespace Nancy.Authentication.OAuth
{
    using System;
    using ModelBinding;

    public class OAuthAuthenticationModule : NancyModule
    {
        public OAuthAuthenticationModule(
            IAuthenticationProvider authenticationProvider,
            IAccessTokenGenerator accessTokenGenerator,
            IAccessTokenPersister accessTokenPersister) : base(OAuth.Configuration.Base)
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

                var cookie =
                    OAuth.DecryptAndValidateCookie<Authorization>(Request, OAuth.Configuration);

                accessTokenPersister.Persist(oauth_token, cookie.State);

                var target =
                    string.Concat(authentication.Redirect_Uri, "?oauth_token=", oauth_token);

                return Response.AsRedirect(target);
            };
        }
    }

    public interface IAccessTokenPersister
    {
        void Persist(string accessToken, string scope);
    }

    public class DefaultAccessTokenPersister : IAccessTokenPersister
    {
        public void Persist(string accessToken, string scope)
        {
            var i = 10;
        }
    }
}