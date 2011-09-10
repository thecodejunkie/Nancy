namespace Nancy.Authentication.OAuth
{
    using System;

    public interface IAuthorizationCodeRequestValidator
    {
        AuthorizationErrorType Validate(AuthorizationCodeRequest request);
    }

    public class DefaultAuthorizationCodeRequestValidator : IAuthorizationCodeRequestValidator
    {
        private readonly IAuthorizationCodeRequestScopeValidator scopeValidator;

        public DefaultAuthorizationCodeRequestValidator(IAuthorizationCodeRequestScopeValidator scopeValidator)
        {
            this.scopeValidator = scopeValidator;
        }

        public AuthorizationErrorType Validate(AuthorizationCodeRequest request)
        {
            if (string.IsNullOrEmpty(request.Client_Id))
            {
                return AuthorizationErrorType.InvalidRequest;
            }

            if (string.IsNullOrEmpty(request.Response_Type) ||
                !request.Response_Type.Equals("code", StringComparison.OrdinalIgnoreCase))
            {
                return AuthorizationErrorType.UnsupportedResponseType;
            }

            if (!this.scopeValidator.Validate(request.Scope))
            {
                return AuthorizationErrorType.InvalidScope;
            }

            if(!string.IsNullOrEmpty(request.Redirect_Uri))
            {
                Uri parsed;
                if (!Uri.TryCreate(request.Redirect_Uri, UriKind.Absolute, out parsed))
                {
                    return AuthorizationErrorType.InvalidRequest;
                }
            }

            return AuthorizationErrorType.None;
        }
    }
}