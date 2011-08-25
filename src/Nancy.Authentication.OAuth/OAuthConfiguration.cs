using Nancy.Cryptography;

namespace Nancy.Authentication.OAuth
{
    /// <summary>
    /// Provides the configuration for the OAuth support
    /// </summary>
    /// <remarks>This class should come with sensible defaults that you can override!</remarks>
    public class OAuthConfiguration
    {
        public OAuthConfiguration()
        {
            this.CryptographyConfiguration = CryptographyConfiguration.Default;
        }

        public string Base { get; set; }
        public string AuthorizationRequestRoute { get; set; }
        public string AuthorizationAllowRoute { get; set; }
        public string AuthorizationDenyRoute { get; set; }
        public string AuthorizationView { get; set; }
        public string AuthenticationRoute { get; set; }

        /// <summary>
        /// Gets or sets the cryptography configuration
        /// </summary>
        public CryptographyConfiguration CryptographyConfiguration { get; set; }
    }
}