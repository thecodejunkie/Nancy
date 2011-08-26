namespace Nancy.Authentication.OAuth
{
    using System;

    public interface IAccessTokenGenerator
    {
        string Generate();
    }

    public class DefaultAccessTokenGenerator : IAccessTokenGenerator
    {
        public string Generate()
        {
            return "Token" + Guid.NewGuid().ToString("N");
        }
    }
}