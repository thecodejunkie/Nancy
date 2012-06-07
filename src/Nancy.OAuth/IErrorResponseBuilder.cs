namespace Nancy.OAuth
{
    using System;
    using System.Collections.Generic;

    public interface IErrorResponseBuilder
    {
        ErrorResponse Build(ErrorType errorType, string state);
    }

    public class DefaultErrorResponseBuilder : IErrorResponseBuilder
    {
        private readonly IDictionary<ErrorType, Tuple<string, string>> errorDescriptions;

        public DefaultErrorResponseBuilder()
        {
            this.errorDescriptions =
                new Dictionary<ErrorType, Tuple<string, string>>
                {
                    { ErrorType.AccessDenied, Tuple.Create("access_denied", "The user denied your request") },
                    { ErrorType.InvalidClient, Tuple.Create("", "") },
                    { ErrorType.InvalidGrant, Tuple.Create("", "") },
                    { ErrorType.InvalidRequest, Tuple.Create("invalid_request", "The request is missing a required parameter, includes an unsupported parameter or parameter value, or is otherwise malformed.") },
                    { ErrorType.InvalidScope, Tuple.Create("invalid_scope", "The requested scope is invalid, unknown, or malformed.") },
                    { ErrorType.ServerError, Tuple.Create("server_error", "The authorization server encountered an unexpected condition which prevented it from fulfilling the request.") },
                    { ErrorType.TemporarilyUnavailable, Tuple.Create("temporarily_unavailable", "The authorization server is currently unable to handle the request due to a temporary overloading or maintenance of the server.") },
                    { ErrorType.UnauthorizedClient, Tuple.Create("unauthorized_client", "The client is not authorized to request an authorization code using this method.") },
                    { ErrorType.UnsupportedGrantType, Tuple.Create("", "") },
                    { ErrorType.UnsupportedResponseType, Tuple.Create("unsupported_response_type", "The authorization server does not support obtaining an authorization code using this method.") }
                };
        }

        public ErrorResponse Build(ErrorType errorType, string state)
        {
            var descriptions =
                this.errorDescriptions[errorType];

            return new ErrorResponse
            {
                Error = descriptions.Item1,
                Error_Description = descriptions.Item2,
                State = state
            };
        }
    }
}