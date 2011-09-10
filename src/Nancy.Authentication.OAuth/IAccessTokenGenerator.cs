namespace Nancy.Authentication.OAuth
{
    using System;

    public interface IAccessTokenGenerator
    {
        AccessToken Generate();
    }

    public class DefaultAccessTokenGenerator : IAccessTokenGenerator
    {
        public AccessToken Generate()
        {
            return new AccessToken
            {
                Access_Token = "Token" + Guid.NewGuid().ToString("N")
            };
        }
    }
}