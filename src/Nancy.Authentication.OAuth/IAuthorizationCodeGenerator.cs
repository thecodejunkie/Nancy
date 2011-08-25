namespace Nancy.Authentication.OAuth
{
    using System;

    public interface IAuthorizationCodeGenerator
    {
        string Generate();
    }

    public class AuthorizationCodeGenerator : IAuthorizationCodeGenerator
    {
        public string Generate()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}