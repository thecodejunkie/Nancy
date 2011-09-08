namespace Nancy.Authentication.OAuth
{
    using System;
    using System.Runtime.Caching;

    public interface IAuthorizationCodeStore
    {
        AuthorizationRequest Retrieve(string code);

        void Store(string code, AuthorizationRequest authorizationRequest);
    }

    public class DefaultAuthorizationCodeStore : IAuthorizationCodeStore
    {
        private readonly MemoryCache cache;

        public DefaultAuthorizationCodeStore()
        {
            this.cache = new MemoryCache("AuthorizationRequestCache");
        }

        public AuthorizationRequest Retrieve(string code)
        {
            return this.cache[code] as AuthorizationRequest;
        }

        public void Store(string code, AuthorizationRequest authorizationRequest)
        {
            this.cache.Add(code, authorizationRequest, DateTime.Now.AddMinutes(10));
        }
    }
}