namespace Nancy.OAuth
{
    using System;
    using System.Collections.Generic;

    public interface IAuthorizationErrorResponseBuilder
    {
        AuthorizationErrorResponse Build(AuthorizationErrorType errorType, AuthorizationRequest request);
    }

    public class AuthorizationErrorResponseBuilder : IAuthorizationErrorResponseBuilder
    {
        private readonly IDictionary<AuthorizationErrorType, Tuple<string, string>> errorDescriptions;

        public AuthorizationErrorResponseBuilder()
        {
            this.errorDescriptions =
                new Dictionary<AuthorizationErrorType, Tuple<string, string>>
                {
                    { AuthorizationErrorType.AccessDenied, Tuple.Create("access_denied", "The user denied your request") },
                    { AuthorizationErrorType.InvalidRequest, Tuple.Create("invalid_request", "The request is missing a required parameter, includes an unsupported parameter or parameter value, or is otherwise malformed.") },
                    { AuthorizationErrorType.InvalidScope, Tuple.Create("invalid_scope", "The requested scope is invalid, unknown, or malformed.") },
                    { AuthorizationErrorType.ServerError, Tuple.Create("server_error", "The authorization server encountered an unexpected condition which prevented it from fulfilling the request.") },
                    { AuthorizationErrorType.TemporarilyUnavailable, Tuple.Create("temporarily_unavailable", "The authorization server is currently unable to handle the request due to a temporary overloading or maintenance of the server.") },
                    { AuthorizationErrorType.UnauthorizedClient, Tuple.Create("unauthorized_client", "The client is not authorized to request an authorization code using this method.") },
                    { AuthorizationErrorType.UnsupportedResponseType, Tuple.Create("unsupported_response_type", "The authorization server does not support obtaining an authorization code using this method.") }
                };
        }

        public AuthorizationErrorResponse Build(AuthorizationErrorType errorType, AuthorizationRequest request)
        {
            var descriptions =
                this.errorDescriptions[errorType];

            return new AuthorizationErrorResponse
            {
                Error = descriptions.Item1,
                Error_Description = descriptions.Item2,
                State = (request != null) ? request.State : string.Empty
            };
        }
    }
}