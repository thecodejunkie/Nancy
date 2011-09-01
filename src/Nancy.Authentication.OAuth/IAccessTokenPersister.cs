namespace Nancy.Authentication.OAuth
{
    using System;
    using System.Collections.Generic;

    public interface IAccessTokenPersister
    {
        void Persist(string client_id, AccessTokenResponse accessToken, string scope);
    }

    public class DefaultAccessTokenPersister : IAccessTokenPersister
    {
        private readonly Dictionary<string, Tuple<AccessTokenResponse, string>> store;

        public DefaultAccessTokenPersister()
        {
            this.store = new Dictionary<string, Tuple<AccessTokenResponse, string>>();
        }

        public void Persist(string client_id, AccessTokenResponse accessToken, string scope)
        {
            this.store[client_id] = Tuple.Create(accessToken, scope);
        }
    }
}