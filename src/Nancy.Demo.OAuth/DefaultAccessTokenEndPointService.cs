namespace Nancy.Demo.OAuth
{
    using System;
    using System.Runtime.Caching;
    using Nancy.OAuth;

    public class DefaultAccessTokenEndPointService : IAccessTokenEndPointService
    {
        private readonly IAccessTokenStore accessTokenStore;
        private readonly IAuthorizationTokenStore authorizationTokenStore;

        public DefaultAccessTokenEndPointService(IAccessTokenStore accessTokenStore, IAuthorizationTokenStore authorizationTokenStore)
        {
            this.accessTokenStore = accessTokenStore;
            this.authorizationTokenStore = authorizationTokenStore;
        }

        public AccessTokenResponse CreateAccessTokenResponse(AccessTokenRequest tokenRequest, NancyContext context)
        {
            this.authorizationTokenStore.Remove(context.CurrentUser.UserName);

            var token =
                string.Concat("access-token-", Guid.NewGuid().ToString("D"));

            this.accessTokenStore.Store(context.CurrentUser.UserName, token);

            return new AccessTokenResponse
            {
                Access_Token = token
            };
        }

        public ValidationResult ValidateRequest(AccessTokenRequest tokenRequest, NancyContext context)
        {
            var authorizationToken =
                this.authorizationTokenStore.Retrieve(context.CurrentUser.UserName);

            if (authorizationToken == null)
            {
                return ErrorType.InvalidGrant;
            }

            return ErrorType.None;
        }
    }

    public interface IAccessTokenStore
    {
        void Store(string key, string value);

        void Remove(string key);

        string Retrieve(string key);
    }

    public class DefaultAccessTokenStore : IAccessTokenStore
    {
        private readonly MemoryCache cache;

        public DefaultAccessTokenStore()
        {
            this.cache = new MemoryCache("Nancy-AuthorizationTokenCache");
        }

        public void Store(string key, string value)
        {
            this.cache.Add(key, value, DateTimeOffset.MaxValue);
        }

        public void Remove(string key)
        {
            if (this.cache.Contains(key))
            {
                this.cache.Remove(key);
            }
        }

        public string Retrieve(string key)
        {
            return this.cache.Contains(key) ? (string)this.cache[key] : null;
        }
    }
}