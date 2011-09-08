namespace Nancy.Authentication.OAuth
{
    using Nancy;
    using ModelBinding;

    public class OAuthAuthorizationModule : NancyModule
    {
        public OAuthAuthorizationModule(
            IAuthorizationViewModelFactory authorizationViewModelFactory,
            IApplicationRepository applicationRepository,
            IAuthorizationCodeGenerator authorizationCodeGenerator,
            IAuthorizationCodeRepository authorizationCodeRepository,
            IAuthorizationRequestStore authorizationRequestStore,
            IAuthorizationCodeStore authorizationCodeStore) : base(OAuth.Configuration.Base)
        {
            OAuth.Configuration.SecureModule(this);

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

                authorizationRequestStore.Store(authorization, this.Context);

                var model = 
                    new { Application = application, Authorization = authorization };

                var viewModel =
                    authorizationViewModelFactory.CreateViewModel(model.AsExpandoObject());

                return View[OAuth.Configuration.AuthorizationView, viewModel]; ;
            };

            Post[OAuth.Configuration.AuthorizationAllowRoute] = parameters =>
            {
                var authorizationCode =
                    authorizationCodeGenerator.Generate();

                var authorizationRequest =
                    authorizationRequestStore.Retrieve(this.Context);

                authorizationCodeRepository.Store(this.Context, authorizationCode);
                authorizationCodeStore.Store(authorizationCode, authorizationRequest);

                var response = 
                    new AuthorizationResponse
                    {
                        Code = authorizationCode,
                        State = authorizationRequest.State
                    };

                return Response.AsRedirect(
                    string.Concat(authorizationRequest.Redirect_Uri, response.AsQueryString()));
            };

            Post[OAuth.Configuration.AuthorizationDenyRoute] = parameters =>
            {
                var authorizationRequest =
                    authorizationRequestStore.Retrieve(this.Context);

                var error = new AuthorizationErrorResponse
                {
                    Error = "access_denied",
                    Error_Description = "The user denied your request",
                    State = authorizationRequest.State
                };

                authorizationRequestStore.Clear(this.Context);

                return Response.AsErrorResponse(error, authorizationRequest.Redirect_Uri);
            };

        }
    }
}