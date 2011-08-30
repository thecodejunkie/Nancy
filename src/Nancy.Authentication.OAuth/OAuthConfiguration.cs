using Nancy.Cryptography;

namespace Nancy.Authentication.OAuth
{
    using System;

    /// <summary>
    /// Provides the configuration for the OAuth support
    /// </summary>
    /// <remarks>This class should come with sensible defaults that you can override!</remarks>
    public class OAuthConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        public OAuthConfiguration()
        {
            this.AuthorizationRequestRoute = "/authorize";
            this.AuthorizationAllowRoute = "/allow";
            this.AuthorizationDenyRoute = "/deny";
            this.AuthorizationView = "authorize";
            this.AuthenticationRoute = "/access_token";
            this.Base = "/oauth";
            this.CryptographyConfiguration = CryptographyConfiguration.Default;
            this.PreRequest = new BeforePipeline();
        }

        public string Base { get; set; }
        public string AuthorizationRequestRoute { get; set; }
        public string AuthorizationAllowRoute { get; set; }
        public string AuthorizationDenyRoute { get; set; }
        public string AuthorizationView { get; set; }
        public string AuthenticationRoute { get; set; }
        public BeforePipeline PreRequest { get; set; }

        /// <summary>
        /// Gets or sets the cryptography configuration
        /// </summary>
        public CryptographyConfiguration CryptographyConfiguration { get; set; }
    }
}