namespace Nancy.OAuth
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Cookies;
    using Helpers;
    using Json;
    using ModelBinding;
    using Responses;
    using Security;

    public class AuthorizationEndPoint : NancyModule
    {
        public AuthorizationEndPoint(IAuthorizationEndPointService service, IAuthorizationErrorResponseBuilder authorizationErrorResponseBuilder) : base("/oauth/authorize")
        {
            this.RequiresAuthentication();

            Get["/"] = parameters =>{
                var request =
                    this.Bind<AuthorizationRequest>();

                var results =
                    service.ValidateRequest(request, this.Context);

                var authorizationView =
                    service.GetAuthorizationView(request, this.Context);

                return View[authorizationView.Item1, authorizationView.Item2];
            };

            Post["/allow"] = parameters => {
                var token =
                    service.GenerateAuthorizationToken(this.Context);

                var response =
                    new AuthorizationResponse
                    {
                        //Code = authorizationCode,
                        //State = authorizationRequest.State
                    };

                return Response.AsRedirect("", RedirectResponse.RedirectType.Found);
            };

            Post["/deny"] = parameters => {
                throw new NotImplementedException();
            };
        }
    }

    public static class QuerystringExtensions
    {
        public static string AsQueryString(this object source)
        {
            var keyValuePairs = source
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetValue(source, null) != null)
                .Select(x => string.Concat(x.Name, "=", HttpUtility.UrlEncode(x.GetValue(source, null).ToString())));

            return string.Concat("?", string.Join("&", keyValuePairs));
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

    public interface IAuthorizationEndPointService
    {
        string GenerateAuthorizationToken(NancyContext context);

        Tuple<string, object> GetAuthorizationView(AuthorizationRequest request, NancyContext context);

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