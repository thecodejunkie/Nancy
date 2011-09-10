namespace Nancy.Authentication.OAuth
{
    using System;
    using Nancy;
    using ModelBinding;

    public class AuthorizationCodeModule : NancyModule
    {
        public AuthorizationCodeModule(
            IViewModelDecorator viewModelDecorator,
            IApplicationRepository applicationRepository,
            IAuthorizationCodeGenerator codeGenerator,
            IAuthorizationRequestStore requestStore,
            IAuthorizationCodeStore codeStore,
            IAuthorizationCodeRequestValidator requestValidator,
            IAuthorizationErrorResponseBuilder errorResponseBuilder) : base(OAuth.Configuration.Base)
        {
            OAuth.Configuration.SecureModule(this);

            Get[OAuth.Configuration.AuthorizationRequestRoute, ctx => OAuth.IsEnabled] = parameters =>
            {
                var authorization =
                    this.Bind<AuthorizationCodeRequest>();

                var validationError =
                    requestValidator.Validate(authorization);

                if (validationError != AuthorizationErrorType.None)
                {
                    return Response.AsErrorResponse(
                        errorResponseBuilder.Build(validationError, authorization), 
                        authorization.Redirect_Uri);
                }

                var application =
                    applicationRepository.GetApplication(authorization.Client_Id);
                
                if (application == null)
                {
                    return Response.AsErrorResponse(
                        errorResponseBuilder.Build(AuthorizationErrorType.ServerError, authorization),
                        authorization.Redirect_Uri);
                }

                if (!application.RedirectUri.Equals(authorization.Redirect_Uri, StringComparison.OrdinalIgnoreCase))
                {
                    return View[OAuth.Configuration.AuthorizationErrorView, new { description = "The provided redirect_uri did not match the registered uri for the application." }];
                }

                requestStore.Store(this.Context.CurrentUser, authorization);

                var model = 
                    new { Application = application, Authorization = authorization };

                var viewModel =
                    viewModelDecorator.CreateViewModel(model.AsExpandoObject());

                return View[OAuth.Configuration.AuthorizationView, viewModel]; ;
            };

            Post[OAuth.Configuration.AuthorizationAllowRoute, ctx => OAuth.IsEnabled] = parameters =>
            {
                var authorizationCode =
                    codeGenerator.Generate();

                var authorizationRequest =
                    requestStore.Retrieve(this.Context.CurrentUser);

                codeStore.Store(this.Context.CurrentUser, authorizationCode, authorizationRequest);
                requestStore.Clear(this.Context.CurrentUser);

                var response = 
                    new AuthorizationResponse
                    {
                        Code = authorizationCode,
                        State = authorizationRequest.State
                    };

                return Response.AsRedirect(
                    string.Concat(authorizationRequest.Redirect_Uri, response.AsQueryString()));
            };

            Post[OAuth.Configuration.AuthorizationDenyRoute, ctx => OAuth.IsEnabled] = parameters =>
            {
                var authorizationRequest =
                    requestStore.Retrieve(this.Context.CurrentUser);

                requestStore.Clear(this.Context.CurrentUser);

                return Response.AsErrorResponse(
                    errorResponseBuilder.Build(AuthorizationErrorType.AccessDenied, authorizationRequest), 
                    authorizationRequest.Redirect_Uri);
            };

        }
    }
}