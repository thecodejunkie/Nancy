namespace Nancy.Authentication.OAuth
{
    using System;

    public interface IAuthenticationProvider
    {
        bool Authenticate(Authentication authentication);
    }

    public class AuthenticationProvider : IAuthenticationProvider
    {
        public bool Authenticate(Authentication authentication)
        {
            return authentication.Client_Id.Equals("NancyApp");
        }
    }
}