namespace Nancy.Demo.OAuth
{
    using System;
    using System.Linq;
    using System.Runtime.Caching;
    using Nancy.OAuth;

    public class DefaultAuthorizationEndPointService : IAuthorizationEndPointService
    {
        private readonly IApplicationStore applicationStore;
        private readonly IAuthorizationTokenStore authorizationTokenStore;

        public DefaultAuthorizationEndPointService(IApplicationStore applicationStore, IAuthorizationTokenStore authorizationTokenStore)
        {
            this.applicationStore = applicationStore;
            this.authorizationTokenStore = authorizationTokenStore;
        }

        public string GenerateAuthorizationToken(NancyContext context)
        {
            var token = 
                string.Concat("authorization-token-", Guid.NewGuid().ToString("D"));

            this.authorizationTokenStore.Store(context.CurrentUser.UserName, token);

            return token;
        }

        public Tuple<string, object> GetAuthorizationView(AuthorizationRequest request, NancyContext context)
        {
            var application =
                this.applicationStore.First(app => app.Id.ToString().Equals(request.ClientId, StringComparison.OrdinalIgnoreCase));

            return new Tuple<string, object>(
                "authorize",
                new AuthorizeViewModel {
                    Body = "View model body",
                    Name = application.Name,
                    Description = application.Description,
                    Permissions = application.Permissions
                });
        }

        public ValidationResult ValidateRequest(AuthorizationRequest request, NancyContext context)
        {
            return new ValidationResult(ErrorType.None);
        }
    }

    public class DefaultAuthorizationTokenStore : IAuthorizationTokenStore
    {
        private readonly MemoryCache cache;

        public DefaultAuthorizationTokenStore()
        {
            this.cache = new MemoryCache("Nancy-AuthorizationTokenCache");
        }

        public void Store(string key, string value)
        {
            this.cache.Add(key, value, DateTimeOffset.Now.AddMinutes(10));
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

    public interface IAuthorizationTokenStore
    {
        void Store(string key, string value);

        void Remove(string key);

        string Retrieve(string key);
    }
}