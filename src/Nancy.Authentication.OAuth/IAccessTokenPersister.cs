namespace Nancy.Authentication.OAuth
{
    using System.Collections.Generic;
    using Security;

    public interface IAccessTokenPersister
    {
        void Persist(IUserIdentity userIdentity, AccessToken tokenResponse, IEnumerable<string> scopes);
    }

    public class DefaultAccessTokenPersister : IAccessTokenPersister
    {
        private readonly IDatabase database;

        public DefaultAccessTokenPersister(IDatabase database)
        {
            this.database = database;
        }

        public void Persist(IUserIdentity userIdentity, AccessToken tokenResponse, IEnumerable<string> scopes)
        {
            this.database.SaveOAuthInformation(
                userIdentity.UserName,
                tokenResponse.Access_Token,
                tokenResponse.Refresh_Token,
                tokenResponse.Expires_In,
                scopes);
        }
    }
}