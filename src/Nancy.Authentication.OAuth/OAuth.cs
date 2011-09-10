namespace Nancy.Authentication.OAuth
{
    using System;

    /*
     * RESOURCES
     *   - Specification http://tools.ietf.org/html/draft-ietf-oauth-v2-20
     *   - OAuth 2.0 @ Facebook.com http://www.sociallipstick.com/?p=239
     */

    public static class OAuth
    {
        /// <summary>
        /// The configuration used by the OAuth provider.
        /// </summary>
        public static OAuthConfiguration Configuration = new OAuthConfiguration();

        public static bool IsEnabled { get; private set; }

        public static void Enable()
        {
            IsEnabled = true;
        }

        public static void Enable(Action<OAuthConfiguration> closure)
        {
            var configuration =
                new OAuthConfiguration();

            closure.Invoke(configuration);

            Enable(configuration);
        }

        public static void Enable(OAuthConfiguration configuration)
        {
            Configuration = configuration;
            IsEnabled = true;
        }
    }
}
