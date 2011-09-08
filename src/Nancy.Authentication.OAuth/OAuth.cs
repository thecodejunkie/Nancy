namespace Nancy.Authentication.OAuth
{
    using System;
    using Nancy.Bootstrapper;


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
        public static OAuthConfiguration Configuration { get; private set; }

        public static void Enable(IApplicationPipelines applicationPipelines)
        {
            Enable(applicationPipelines, new OAuthConfiguration());
        }

        public static void Enable(IApplicationPipelines applicationPipelines, Action<OAuthConfiguration> closure)
        {
            var configuration =
                new OAuthConfiguration();

            closure.Invoke(configuration);

            Enable(applicationPipelines, configuration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationPipelines"></param>
        /// <param name="configuration"></param>
        public static void Enable(IApplicationPipelines applicationPipelines, OAuthConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
