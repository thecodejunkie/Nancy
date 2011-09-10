namespace Nancy.Authentication.OAuth
{
    using System;
    using System.Runtime.Caching;
    using Security;

    public interface IAuthorizationCodeStore
    {
        Tuple<AuthorizationCodeRequest, IUserIdentity> Retrieve(string code);

        void Store(IUserIdentity userIdentity, string code, AuthorizationCodeRequest request);
    }

    public class DefaultAuthorizationCodeStore : IAuthorizationCodeStore
    {
        private readonly MemoryCache cache;

        public DefaultAuthorizationCodeStore()
        {
            this.cache = new MemoryCache("AuthorizationRequestCache");
        }

        public Tuple<AuthorizationCodeRequest, IUserIdentity> Retrieve(string code)
        {
            return this.cache[code] as Tuple<AuthorizationCodeRequest, IUserIdentity>;
        }

        public void Store(IUserIdentity userIdentity, string code, AuthorizationCodeRequest request)
        {
            this.cache.Add(code, Tuple.Create(request, userIdentity), DateTime.Now.AddMinutes(10));
        }
    }
}