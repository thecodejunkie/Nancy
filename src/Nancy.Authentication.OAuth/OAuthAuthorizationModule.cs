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

                // Get stored information about the application that is trying to authorize
                // not 100% sure here as the information that is stored about the app could
                // be impl specific.. but maybe this should just store the essentials and
                // then let you bring in more impl specific info for the view model ?!
                var application =
                    applicationRepository.GetApplication(authorization.Client_Id);

                // TODO: Should handle invalid client_id, i.e when no application matched the id

                // The idea here is that based on the retrieved application info, you should be
                // able to create a viewModel for the stuff that should be shown on the page where
                // the user gets to allow/deny access.
                var model =
                    authorizationViewModelFactory.CreateViewModel(authorization, application);

                // Render the view with the viewModel that's been created
                var response =
                    (Response)View[OAuth.Configuration.AuthorizationView, model];

                // Store the authorization info in a cookie because it's needed in the routes
                // that handels when the user Allows or Denies access
                OAuth.StoreAuthorization(response, authorization);

                return response;
            };

            Post[OAuth.Configuration.AuthorizationAllowRoute] = parameters =>
            {
                // Generate the authorization code that the application will have to send back
                // in to get the oauth_token later on
                var code =
                    authorizationCodeGenerator.Generate();

                var authorization =
                    OAuth.DecryptAndValidateCookie<Authorization>(this.Request, OAuth.Configuration);

                // Should store the actual client_id as key
                authorizationCodeRepository.Store(string.Empty, code);

                // Should redirect to Authorization.Redirect_Uri
                // and pass along the generated code and Authorization.State

                // Shouldn't the code be stored, with the scope so that the scope
                // can be persisted with the oauth_token later on. The application
                // needs to know what permission set a token has been granted.

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