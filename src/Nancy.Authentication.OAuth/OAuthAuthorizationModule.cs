namespace Nancy.Authentication.OAuth
{
    using System;
    using Nancy.ModelBinding;
    using Security;

    public class OAuthAuthorizationModule : NancyModule
    {
        public OAuthAuthorizationModule(
            IAuthorizationViewModelFactory authorizationViewModelFactory,
            IApplicationRepository applicationRepository,
            IAuthorizationCodeGenerator authorizationCodeGenerator,
            IAuthorizationCodeRepository authorizationCodeRepository) : base(OAuth.Configuration.Base)
        {
            this.RequiresAuthentication();

            Get[OAuth.Configuration.AuthorizationRequestRoute] = parameters =>
            {
                var authorization =
                    this.Bind<Authorization>();

                var application =
                    applicationRepository.GetApplication(authorization.Client_Id);

                // TODO: Should handle invalid client_id, i.e when no application matched the id

                var model =
                    authorizationViewModelFactory.CreateViewModel(authorization, application);

                var response =
                    (Response)View[OAuth.Configuration.AuthorizationView, model];

                OAuth.StoreAuthorization(response, authorization);

                return response;
            };

            Post[OAuth.Configuration.AuthorizationAllowRoute] = parameters =>
            {
                var code =
                    authorizationCodeGenerator.Generate();

                var authorization =
                    OAuth.DecryptAndValidateCookie<Authorization>(this.Request, OAuth.Configuration);

                // Should store the actual client_id as key
                authorizationCodeRepository.Store(string.Empty, code);

                // Should redirect to Authorization.Redirect_Uri
                // and pass along the generated code and Authorization.State

                var targetUrl = 
                    string.Concat(authorization.Redirect_Uri,"?code=", code);

                if (!string.IsNullOrEmpty(authorization.State))
                {
                    targetUrl = string.Concat(targetUrl, "&state=", authorization.State);
                }

                return Response.AsRedirect(targetUrl);
            };

            Post[OAuth.Configuration.AuthorizationDenyRoute] = parameters =>
            {
                var authorization =
                    OAuth.DecryptAndValidateCookie<Authorization>(this.Request, OAuth.Configuration);

                // Provide a better error ;)
                var targetUrl =
                    string.Concat(authorization.Redirect_Uri, "?error=CantTouchThis");

                return Response.AsRedirect(targetUrl);
            };

        }
    }
}