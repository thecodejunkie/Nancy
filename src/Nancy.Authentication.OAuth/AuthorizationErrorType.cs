namespace Nancy.Authentication.OAuth
{
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
}