namespace Nancy.OAuth
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Cookies;
    using Json;
    using ModelBinding;
    using Responses;
    using Security;

    public class AuthorizationEndPoint : NancyModule
    {
        /// <summary>
        /// The base address and the various end-points in the module should be grabbed from an
        /// OAuthConfiguration and should provide sensible default-values that can be overriden
        /// when you enable OAuth in your application.
        /// </summary>
        public AuthorizationEndPoint(
            IAuthorizationEndPointService service, 
            IAuthorizationErrorResponseBuilder errorResponseBuilder) : base("/oauth/authorize")
        {
            this.RequiresAuthentication();

            Get["/"] = parameters =>{
                var request =
                    this.Bind<AuthorizationRequest>();

                var results =
                    service.ValidateRequest(request, this.Context);

                if (!results.IsValid)
                {
                    return Response.AsErrorResponse(errorResponseBuilder.Build(results.ErrorType, request), request.RedirectUrl);
                }

                // Use something more secure than the username
                this.Session[Context.CurrentUser.UserName] = request;

                var authorizationView =
                    service.GetAuthorizationView(request, this.Context);

                return View[authorizationView.Item1, authorizationView.Item2];
            };

            Post["/allow"] = parameters => {
                var token =
                    service.GenerateAuthorizationToken(this.Context);

                var request =
                    this.Session[Context.CurrentUser.UserName] as AuthorizationRequest;

                if (request == null)
                {
                    return HttpStatusCode.InternalServerError;
                }

                var response =
                    new AuthorizationResponse
                    {
                        Code = token,
                        State = request.State
                    };

                // TODO: Perhaps use an UriBuilder instead?
                var url =
                    string.Concat(request.RedirectUrl, response.AsQueryString());

                return Response.AsRedirect(url, RedirectResponse.RedirectType.Found);
            };

            Post["/deny"] = parameters => {
                var request =
                    this.Session[Context.CurrentUser.UserName] as AuthorizationRequest;

                return request == null ? 
                    HttpStatusCode.InternalServerError : 
                    Response.AsErrorResponse(errorResponseBuilder.Build(AuthorizationErrorType.AccessDenied, request), request.RedirectUrl);
            };
        }
    }

    public enum AuthorizationErrorType
    {
        None,
        InvalidRequest,
        UnauthorizedClient,
        AccessDenied,
        UnsupportedResponseType,
        InvalidScope,
        ServerError,
        TemporarilyUnavailable
    }

    public class AuthorizationResponse
    {
        public string Code { get; set; }

        public string State { get; set; }
    }

    public class AuthorizationErrorResponse
    {
        public string Error { get; set; }

        public string Error_Description { get; set; }

        public string State { get; set; }
    }

    public class AuthorizeViewModel
    {
        public string Body { get; set; }

        public IEnumerable<string> Permissions { get; set; }
    }

    /// <summary>
    /// This interface might be split up into smaller pieces later on, but for spiking purposes it's all left in
    /// this single definition.
    /// </summary>
    public interface IAuthorizationEndPointService
    {
        /// <summary>
        /// Generate a token. Should this be responsible for storing it as well? I am kinda tempted to say that it
        /// should be responsible for storing it as well, to make the code generation automic. However, what happens
        /// if an error happens, after the code's been generated (and stored)? Would leave a dead code in the
        /// storage.
        /// 
        /// However, the specification states "The authorization code MUST expire shortly after it is issued to 
        /// mitigate the risk of leaks.  A maximum authorization code lifetime of 10 minutes is RECOMMENDED." so
        /// there should probably be a TTL configuration passed in here to. It would then be up to the storage
        /// mechanism to make sure the code is made invalid/cleaned up once the TTL expired. The strategy would
        /// probably vary depending on the implementation, so imposing a strict schema for doing this might not
        /// be a wise thing to do.
        /// </summary>
        string GenerateAuthorizationToken(NancyContext context);

        /// <summary>
        /// Returns the name of the view that should be used to ask the user to approve the application. It also
        /// returns the view model that should be used when rendering the view. Sending in the AuthorizationRequest
        /// lets the user add things like the Scope information to their view model. For instance you could also use
        /// the client id to fetch information about the application that is requesting approval.
        /// </summary>
        Tuple<string, object> GetAuthorizationView(AuthorizationRequest request, NancyContext context);

        /// <summary>
        /// Thing you are going to want to validate are the client_id, redirect_url (is it valid? does it match the
        /// one that's registered for the application with client_id) and so on
        /// </summary>
        AuthorizationRequestValidationResult ValidateRequest(AuthorizationRequest request, NancyContext context);
    }

    public class DefaultAuthorizationEndPointService : IAuthorizationEndPointService
    {
        public string GenerateAuthorizationToken(NancyContext context)
        {
            return string.Concat("authorization-token-", Guid.NewGuid().ToString());
        }

        public Tuple<string, object> GetAuthorizationView(AuthorizationRequest request, NancyContext context)
        {
            return new Tuple<string, object>(
                "authorize.sshtml",
                new AuthorizeViewModel {
                    Body = "View model body",
                    Permissions = new [] { "Read", "Write", "Delete"}
                }
            );
        }

        public AuthorizationRequestValidationResult ValidateRequest(AuthorizationRequest request, NancyContext context)
        {
            return new AuthorizationRequestValidationResult(true, AuthorizationErrorType.None);
        }
    }

    public class AuthorizationRequestValidationResult
    {
        public AuthorizationRequestValidationResult(bool isValid, AuthorizationErrorType errorType)
        {
            IsValid = isValid;
            ErrorType = errorType;
        }

        public bool IsValid { get; private set; }

        public AuthorizationErrorType ErrorType { get; set; }
    }
}