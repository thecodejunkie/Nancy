namespace Nancy.Authentication.OAuth
{
    using System;

    public interface IAccessTokenGenerator
    {
        AccessTokenResponse Generate();
    }

    public class DefaultAccessTokenGenerator : IAccessTokenGenerator
    {
        public AccessTokenResponse Generate()
        {
            return new AccessTokenResponse
            {
                Access_Token = "Token" + Guid.NewGuid().ToString("N")
            };
        }
    }
}