using System;
namespace Nancy.Authentication.OAuth
{
    using System.Runtime.Caching;

    /// <summary>
    /// Not sure if this should be taking in the context or just the NancyContext.CurrentUser. I think
    /// the context gives the most options.
    /// </summary>
    public interface IAuthorizationRequestStore
    {
        void Clear(NancyContext context);

        AuthorizationRequest Retrieve(NancyContext context);

        void Store(AuthorizationRequest authorizationRequest, NancyContext context);
    }

    public class DefaultAuthorizationRequestStore : IAuthorizationRequestStore
    {
        private readonly MemoryCache cache;

        public DefaultAuthorizationRequestStore()
        {
            this.cache = new MemoryCache("AuthorizationRequestCache");
        }

        public void Clear(NancyContext context)
        {
            this.cache.Remove(context.CurrentUser.UserName);
        }

        public AuthorizationRequest Retrieve(NancyContext context)
        {
            return this.cache[context.CurrentUser.UserName] as AuthorizationRequest;
        }

        public void Store(AuthorizationRequest authorizationRequest, NancyContext context)
        {
            this.cache.Add(context.CurrentUser.UserName, authorizationRequest, DateTimeOffset.Now.Add(new TimeSpan(0, 10, 0)));
        }
    }
}