namespace Nancy.Authentication.OAuth
{
    using System.Collections.Generic;

    public interface IAuthorizationCodeRepository
    {
        void Store(AuthorizationRequest authorizationRequest, string code);
    }

    public class AuthorizationCodeRepository : IAuthorizationCodeRepository
    {
        private readonly Dictionary<AuthorizationRequest, string> store;

        public AuthorizationCodeRepository()
        {
            this.store = new Dictionary<AuthorizationRequest, string>();
        }

        public void Store(AuthorizationRequest authorizationRequest, string code)
        {
            this.store.Add(authorizationRequest, code);
        }
    }
}