namespace Nancy.Authentication.OAuth
{
    using System;

    public interface IAuthenticationProvider
    {
        string Authenticate(Authentication authentication);
    }

    public class AuthenticationProvider : IAuthenticationProvider
    {
        public string Authenticate(Authentication authentication)
        {
            return "Token#" + Guid.NewGuid().ToString("N");
        }
    }
}