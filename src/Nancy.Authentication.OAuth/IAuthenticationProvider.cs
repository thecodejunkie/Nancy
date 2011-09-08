namespace Nancy.Authentication.OAuth
{
    using System;

    public interface IAuthenticationProvider
    {
        bool Authenticate(AccessTokenRequest accessTokenRequest);
    }

    public class AuthenticationProvider : IAuthenticationProvider
    {
        public bool Authenticate(AccessTokenRequest accessTokenRequest)
        {
            return accessTokenRequest.Client_Id.Equals("NancyApp", StringComparison.OrdinalIgnoreCase);
        }
    }
}