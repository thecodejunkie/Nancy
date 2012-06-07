namespace Nancy.OAuth
{
    public enum ErrorType
    {
        None,
        AccessDenied,
        InvalidClient,
        InvalidGrant,
        InvalidRequest,
        InvalidScope,
        ServerError,
        TemporarilyUnavailable,
        UnauthorizedClient,
        UnsupportedGrantType,
        UnsupportedResponseType,               
    }
}