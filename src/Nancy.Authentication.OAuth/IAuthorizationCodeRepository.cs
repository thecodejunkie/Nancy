namespace Nancy.Authentication.OAuth
{
    using System;
    using System.Collections.Specialized;

    public interface IAuthorizationCodeRepository
    {
        void Store(string clientId, string code);
    }

    public class AuthorizationCodeRepository : IAuthorizationCodeRepository
    {
        private readonly NameValueCollection store;

        public AuthorizationCodeRepository()
        {
            this.store = new NameValueCollection();
        }

        public void Store(string clientId, string code)
        {
            this.store.Add(clientId, code);
        }
    }
}