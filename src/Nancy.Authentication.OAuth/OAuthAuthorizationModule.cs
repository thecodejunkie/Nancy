namespace Nancy.Authentication.OAuth
{
    using System;
    using Nancy;
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
            //this.Before = OAuth.Configuration.PreRequest;

            Get[OAuth.Configuration.AuthorizationRequestRoute] = parameters =>
            {
                var authorization =
                    this.Bind<Authorization>();

                if (!authorization.IsValid())
                {
                    //if (string.IsNullOrEmpty(authorization.Redirect_Uri))
                    //{
                    //    return HttpStatusCode.BadRequest;
                    //}

                    var error = new AuthorizationErrorResponse
                    {
                        Error = "invalid_request",
                        Error_Description = "The request is missing a required parameter, includes an unsupported parameter or parameter value, or is otherwise malformed.",
                        State = authorization.State
                    };

                    return Response.AsErrorResponse(error, authorization.Redirect_Uri);
                }

                // Get stored information about the application that is trying to authorize
                // not 100% sure here as the information that is stored about the app could
                // be impl specific.. but maybe this should just store the essentials and
                // then let you bring in more impl specific info for the view model ?!
                var application =
                    applicationRepository.GetApplication(authorization.Client_Id);
                
                if (application == null)
                {
                    var error = new AuthorizationErrorResponse
                    {
                        Error = "unauthorized_client",
                        Error_Description = "The client is not authorized to request an authorization code using this method.",
                        State = authorization.State
                    };

                    return Response.AsErrorResponse(error, authorization.Redirect_Uri);
                }

                var model = 
                    new { Application = application, Authorization = authorization };

                var viewModel =
                    authorizationViewModelFactory.CreateViewModel(model.AsExpandoObject());

                var response =
                    (Response)View[OAuth.Configuration.AuthorizationView, viewModel];

                OAuth.StoreAuthorization(response, authorization);

                return response;
            };

            Post[OAuth.Configuration.AuthorizationAllowRoute] = parameters =>
            {
                var authorizationCode =
                    authorizationCodeGenerator.Generate();

                var authorization =
                    OAuth.DecryptAndValidateCookie<Authorization>(this.Request, OAuth.Configuration);

                authorizationCodeRepository.Store(authorization.Client_Id, authorizationCode);

                var targetUrl =
                    string.Concat(authorization.Redirect_Uri, "?code=", authorizationCode);

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

                var error = new AuthorizationErrorResponse
                {
                    Error = "access_denied",
                    Error_Description = "The user denied your request",
                    State = authorization.State
                };

                return Response.AsErrorResponse(error, authorization.Redirect_Uri);
            };

        }
    }
}