namespace Nancy.Authentication.OAuth
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public interface IDatabase
    {
        void SaveOAuthInformation(string userName, string accessToken, string refreshToken, int? expiresIn, IEnumerable<string> scopes);
    }

    public class Database : IDatabase
    {
        private readonly ConcurrentDictionary<string, Func<UserOAuthInformation>> store;

        public Database()
        {
            this.store = new ConcurrentDictionary<string, Func<UserOAuthInformation>>();
        }

        public void SaveOAuthInformation(string userName, string accessToken, string refreshToken, int? expiresIn, IEnumerable<string> scopes)
        {
            this.store.GetOrAdd(userName, 
                () => new UserOAuthInformation
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        ExpiresIn = expiresIn,
                        Scopes = scopes
                    });
        }

        private class UserOAuthInformation
        {
            public string AccessToken { get; set; }

            public int? ExpiresIn { get; set; }

            public string RefreshToken { get; set; }

            public IEnumerable<string> Scopes { get; set; }
        }
    }
}