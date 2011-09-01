namespace Nancy.Authentication.OAuth
{
    using System;
    using Nancy;
    using Nancy.ModelBinding;

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
                    this.Bind<AuthorizationRequest>();

                if (!authorization.IsValid())
                {
                    var error = new AuthorizationErrorResponse
                    {
                        Error = "invalid_request",
                        Error_Description = "The request is missing a required parameter, includes an unsupported parameter or parameter value, or is otherwise malformed.",
                        State = authorization.State
                    };

                    return Response.AsErrorResponse(error, authorization.Redirect_Uri);
                }

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

                var authorizationRequest =
                    OAuth.DecryptAndValidateCookie<AuthorizationRequest>(this.Request, OAuth.Configuration);

                authorizationCodeRepository.Store(authorizationRequest, authorizationCode);

                var response = 
                    new AuthorizationResponse
                    {
                        Code = authorizationCode,
                        State = authorizationRequest.State
                    };

                return Response.AsRedirect(
                    string.Concat(authorizationRequest.Redirect_Uri,
                    response.AsQueryString()));
            };

            Post[OAuth.Configuration.AuthorizationDenyRoute] = parameters =>
            {
                var authorizationRequest =
                    OAuth.DecryptAndValidateCookie<AuthorizationRequest>(this.Request, OAuth.Configuration);

                var error = new AuthorizationErrorResponse
                {
                    Error = "access_denied",
                    Error_Description = "The user denied your request",
                    State = authorizationRequest.State
                };

                return Response.AsErrorResponse(error, authorizationRequest.Redirect_Uri);
            };

        }
    }
}