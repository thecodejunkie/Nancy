namespace Nancy.Authentication.OAuth
{
    using System;
    using ModelBinding;

    public interface IAccessTokenRequestValidator
    {
        bool Validate(AccessTokenRequest request, string redirectUrl);
    }

    public class DefaultAccessTokenRequestValidator : IAccessTokenRequestValidator
    {
        public bool Validate(AccessTokenRequest request, string redirectUrl)
        {
            throw new NotImplementedException();
        }
    }

    public class OAuthAccessTokenModule : NancyModule
    {
        public OAuthAccessTokenModule(
            IAuthenticationProvider authenticationProvider,
            IAccessTokenGenerator tokenGenerator,
            IAccessTokenPersister tokenPersister,
            IAuthorizationCodeStore codeStore) : base(OAuth.Configuration.Base)
        {
            Post[OAuth.Configuration.AuthenticationRoute, ctx => OAuth.IsEnabled] = parameters =>
            {
                var accessTokenRequest =
                    this.Bind<AccessTokenRequest>();

                if (!accessTokenRequest.IsValid())
                {
                    // TODO: Handle invalid query
                }

                var authorizationRequest =
                    codeStore.Retrieve(accessTokenRequest.Code);

                if (!string.IsNullOrEmpty(authorizationRequest.Item1.Redirect_Uri))
                {
                    if (!authorizationRequest.Item1.Redirect_Uri.Equals(accessTokenRequest.Redirect_Uri, StringComparison.OrdinalIgnoreCase))
                    {
                        // This is an error
                    }
                }

                var isAuthenticated = 
                    authenticationProvider.Authenticate(accessTokenRequest);

                if (!isAuthenticated)
                {
                    // TODO: Handle invalid authentication, is Unauthorized correct response?!
                    return HttpStatusCode.Unauthorized;
                }

                var token =
                    tokenGenerator.Generate();

                tokenPersister.Persist(authorizationRequest.Item2, token, authorizationRequest.Item1.Scope);

                var response =
                    token.AsExpandoObject();

                response.State = 
                    authorizationRequest.Item1.State ?? string.Empty;

                return Response
                    .AsStatusCode(HttpStatusCode.OK)
                    .WithJsonBody((object)response)
                    .WithHeader("Content-Type", "application/x-www-form-urlencoded")
                    .WithNoCache();
            };
        }
    }
}