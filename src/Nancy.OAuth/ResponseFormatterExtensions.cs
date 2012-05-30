namespace Nancy.OAuth
{
    public static class ResponseFormatterExtensions
    {
        public static Response AsErrorResponse(this IResponseFormatter source, AuthorizationErrorResponse error, string redirectUri)
        {
            return source.AsRedirect(string.Concat(redirectUri, error.AsQueryString()));
        }
    }
}