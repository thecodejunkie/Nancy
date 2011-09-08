namespace Nancy.Authentication.OAuth
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Caching;

    public interface IAuthorizationCodeRepository
    {
        void Store(NancyContext context, string code);

        void Store(NancyContext context, string code, TimeSpan timeToLive);
    }

    public class AuthorizationCodeRepository : IAuthorizationCodeRepository
    {
        private readonly MemoryCache cache;

        public AuthorizationCodeRepository()
        {
            this.cache = new MemoryCache("AuthorizationCodeCache");
        }

        public void Store(NancyContext context, string code)
        {
            this.Store(context, code, new TimeSpan(0, 10, 0));
        }

        public void Store(NancyContext context, string code, TimeSpan timeToLive)
        {
            this.cache.Add(context.CurrentUser.UserName, code, DateTimeOffset.Now.Add(timeToLive));
        }
    }
}