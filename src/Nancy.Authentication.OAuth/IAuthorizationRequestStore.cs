namespace Nancy.Authentication.OAuth
{
    using System;
    using System.Runtime.Caching;
    using Security;

    /// <summary>
    /// Not sure if this should be taking in the context or just the NancyContext.CurrentUser. I think
    /// the context gives the most options.
    /// </summary>
    public interface IAuthorizationRequestStore
    {
        void Clear(IUserIdentity userIdentity);

        AuthorizationCodeRequest Retrieve(IUserIdentity userIdentity);

        void Store(IUserIdentity userIdentity, AuthorizationCodeRequest request);
    }

    public class DefaultAuthorizationRequestStore : IAuthorizationRequestStore
    {
        private readonly MemoryCache cache;

        public DefaultAuthorizationRequestStore()
        {
            this.cache = new MemoryCache("AuthorizationRequestCache");
        }

        public void Clear(IUserIdentity userIdentity)
        {
            this.cache.Remove(userIdentity.UserName);
        }

        public AuthorizationCodeRequest Retrieve(IUserIdentity userIdentity)
        {
            return this.cache[userIdentity.UserName] as AuthorizationCodeRequest;
        }

        public void Store(IUserIdentity userIdentity, AuthorizationCodeRequest request)
        {
            this.cache.Add(userIdentity.UserName, request, DateTimeOffset.Now.Add(new TimeSpan(0, 10, 0)));
        }
    }
}