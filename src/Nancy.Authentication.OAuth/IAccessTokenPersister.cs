namespace Nancy.Authentication.OAuth
{
    using System;
    using System.Collections.Generic;

    public interface IAccessTokenPersister
    {
        void Persist(string client_id, AccessToken accessToken, string scope);
    }

    public class DefaultAccessTokenPersister : IAccessTokenPersister
    {
        private readonly Dictionary<string, Tuple<AccessToken, string>> store;

        public DefaultAccessTokenPersister()
        {
            this.store = new Dictionary<string, Tuple<AccessToken, string>>();
        }

        public void Persist(string client_id, AccessToken accessToken, string scope)
        {
            this.store[client_id] = Tuple.Create(accessToken, scope);
        }
    }
}