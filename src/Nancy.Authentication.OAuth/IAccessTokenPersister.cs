namespace Nancy.Authentication.OAuth
{
    using System;
    using System.Collections.Generic;

    public interface IAccessTokenPersister
    {
        void Persist(NancyContext context, AuthorizationRequest authorizationRequest, AccessTokenResponse accessToken);
    }

    public class DefaultAccessTokenPersister : IAccessTokenPersister
    {
        private readonly Dictionary<string, Tuple<AccessTokenResponse, string>> store;

        public DefaultAccessTokenPersister()
        {
            this.store = new Dictionary<string, Tuple<AccessTokenResponse, string>>();
        }

        public void Persist(NancyContext context, AuthorizationRequest authorizationRequest, AccessTokenResponse accessToken)
        {
            this.store[authorizationRequest.Client_Id] = Tuple.Create(accessToken, authorizationRequest.Scope);
        }
    }
}