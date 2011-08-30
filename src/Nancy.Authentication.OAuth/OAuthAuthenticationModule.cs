namespace Nancy.Authentication.OAuth
{
    using System;
    using System.Collections.Generic;
    using ModelBinding;

    public class OAuthAuthenticationModule : NancyModule
    {
        public OAuthAuthenticationModule(
            IAuthenticationProvider authenticationProvider,
            IAccessTokenGenerator accessTokenGenerator,
            IAccessTokenPersister accessTokenPersister) : base(OAuth.Configuration.Base)
        {
            this.Before = OAuth.Configuration.PreRequest;

            // Should this be a POST? Section 3.2 of the specification indicates it should
            Get[OAuth.Configuration.AuthenticationRoute] = parameters =>
            {
                var authentication =
                    this.Bind<Authentication>();

                if (!authentication.IsValid())
                {
                    // TODO: Handle invalid query
                }

                var cookie =
                    OAuth.DecryptAndValidateCookie<Authorization>(Request, OAuth.Configuration);

                if (!string.IsNullOrEmpty(cookie.Redirect_Uri))
                {
                    if (!cookie.Redirect_Uri.Equals(authentication.Redirect_Uri, StringComparison.OrdinalIgnoreCase))
                    {
                        // This is an error
                    }
                }

                var isAuthenticated = 
                    authenticationProvider.Authenticate(authentication);

                if (!isAuthenticated)
                {
                    // TODO: Handle invalid authentication, is Unauthorized correct response?!
                    return HttpStatusCode.Unauthorized;
                }

                var token =
                    accessTokenGenerator.Generate();

                accessTokenPersister.Persist(cookie.Client_Id, token, cookie.Scope);

                // Should JSON serialize this into the response body
                var responseToken = new AccessToken
                {
                    Access_Token = token.Access_Token,
                    State = cookie.State ?? string.Empty
                };

                return Response
                    .AsStatusCode(HttpStatusCode.OK)
                    .WithJsonBody((object) responseToken.AsExpandoObject())
                    .WithNoCache();
            };
        }
    }

    public class AccessToken
    {
        public string Access_Token { get; set; }
        
        public string Token_type { get; set; }
        
        public int Expires_In { get; set; } // Make nullable and update AsExpandoObject to exclude them
        
        public string Refresh_Token { get; set; }

        public string State { get; set; }
    }
}