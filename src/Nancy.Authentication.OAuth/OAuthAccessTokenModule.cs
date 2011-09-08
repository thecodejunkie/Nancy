namespace Nancy.Authentication.OAuth
{
    using System;
    using System.Collections.Generic;
    using ModelBinding;

    public class OAuthAccessTokenModule : NancyModule
    {
        public OAuthAccessTokenModule(
            IAuthenticationProvider authenticationProvider,
            IAccessTokenGenerator accessTokenGenerator,
            IAccessTokenPersister accessTokenPersister,
            IAuthorizationCodeStore authorizationCodeStore) : base(OAuth.Configuration.Base)
        {
            Post[OAuth.Configuration.AuthenticationRoute] = parameters =>
            {
                var accessTokenRequest =
                    this.Bind<AccessTokenRequest>();

                if (!accessTokenRequest.IsValid())
                {
                    //invalid_request
                    //   The request is missing a required parameter, includes an
                    //   unsupported parameter or parameter value, repeats a
                    //   parameter, includes multiple credentials, utilizes more
                    //   than one mechanism for authenticating the client, or is
                    //   otherwise malformed.
                    // TODO: Handle invalid query
                }

                //var cookie =
                //    OAuth.DecryptAndValidateCookie<AuthorizationRequest>(Request, OAuth.Configuration);

                var authorizationRequest =
                    authorizationCodeStore.Retrieve(accessTokenRequest.Code);

                if (!string.IsNullOrEmpty(authorizationRequest.Redirect_Uri))
                {
                    if (!authorizationRequest.Redirect_Uri.Equals(accessTokenRequest.Redirect_Uri, StringComparison.OrdinalIgnoreCase))
                    {
                        // This is an error
                    }
                }

                var isAuthenticated = 
                    authenticationProvider.Authenticate(accessTokenRequest);

                if (!isAuthenticated)
                {
                    //invalid_client
                    //   Client authentication failed (e.g. unknown client, no
                    //   client authentication included, multiple client
                    //   authentications included, or unsupported authentication
                    //   method).  The authorization server MAY return an HTTP 401
                    //   (Unauthorized) status code to indicate which HTTP
                    //   authentication schemes are supported.  If the client
                    //   attempted to authenticate via the "Authorization" request
                    //   header field, the authorization server MUST respond with
                    //   an HTTP 401 (Unauthorized) status code, and include the
                    //   "WWW-Authenticate" response header field matching the
                    //   authentication scheme used by the client.

                    // TODO: Handle invalid authentication, is Unauthorized correct response?!
                    return HttpStatusCode.Unauthorized;
                }

                var token =
                    accessTokenGenerator.Generate();

                accessTokenPersister.Persist(this.Context, authorizationRequest, token);

                var responseToken = new AccessTokenResponse()
                {
                    Access_Token = token.Access_Token,
                    State = authorizationRequest.State ?? string.Empty
                };

                // Set content-type to application/x-www-form-urlencoded
                return Response
                    .AsStatusCode(HttpStatusCode.OK)
                    .WithJsonBody((object)responseToken.AsExpandoObject())
                    .WithNoCache();
            };
        }
    }

    public class AccessTokenResponse
    {
        public string Access_Token { get; set; }
        
        public string Token_type { get; set; }
        
        public int? Expires_In { get; set; } // Make nullable and update AsExpandoObject to exclude them
        
        public string Refresh_Token { get; set; }

        public string State { get; set; }
    }
}