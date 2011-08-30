namespace Nancy.Authentication.OAuth
{
    using System;

    public interface IAuthorizationCodeGenerator
    {
        string Generate();
    }

    public class DefaultAuthorizationCodeGenerator : IAuthorizationCodeGenerator
    {
        public string Generate()
        {
            return "Code" + Guid.NewGuid().ToString("N");
        }
    }
}