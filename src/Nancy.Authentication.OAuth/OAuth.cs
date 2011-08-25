using Nancy.Cryptography;

namespace Nancy.Authentication.OAuth
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Nancy.Cookies;
    using Nancy.Bootstrapper;
    using Nancy.Security;


    /*
     * RESOURCES
     *   - Specification http://tools.ietf.org/html/draft-ietf-oauth-v2-20
     *   - OAuth 2.0 @ Facebook.com http://www.sociallipstick.com/?p=239
     * 
     * 
     * NOTES
     *   - First you authorize
     *      - Pass in the members of IAuthorizationRequest as querystring parameters
     *      - Return to the redirecturi, by appending code and state, from IAuthorizationResponse, in the querystring
     *   - Then you request an access token once you are authorized
     *      - 
     *   - The token should be appended, using oauth_token querystring parameter, on each request to a restricted resource
     *   
     * 
     * 
     */
    
    public class Authorization
    {
        public string Client_Id { get; set; }
        public string Redirect_Uri { get; set; }
        public string Scope { get; set; }
        public string State { get; set; }
    }

    public class Authentication
    {
        public string Client_Id { get; set; }
        public string Client_Secret { get; set; }
        public string Code { get; set; }
        public string Redirect_Uri { get; set; }
    }

    public static class OAuth
    {
        /// <summary>
        /// The configuration used by the OAuth provider.
        /// </summary>
        public static OAuthConfiguration Configuration { get; private set; }

        private const string OAuthAuthorizationCookieName = "_ncoa";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationPipelines"></param>
        /// <param name="configuration"></param>
        public static void Enable(IApplicationPipelines applicationPipelines, OAuthConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static void StoreAuthorization(Response response, Authorization authorization)
        {
            var authorizationCookie =
                BuildCookie(authorization, Configuration);

            response.AddCookie(authorizationCookie);
        }

        /// <summary>
        /// Build the forms authentication cookie
        /// </summary>
        /// <param name="configuration">Current configuration</param>
        /// <returns>Nancy cookie instance</returns>
        private static INancyCookie BuildCookie<T>(T value, OAuthConfiguration configuration)
        {
            var cookieContents = EncryptAndSignCookie(
                Serialize(value), 
                configuration
            );

            var cookie = 
                new NancyCookie(OAuthAuthorizationCookieName, cookieContents, true) { Expires = null };

            return cookie;
        }

        /// <summary>
        /// Encrypt and sign the cookie contents
        /// </summary>
        /// <param name="cookieValue">Plain text cookie value</param>
        /// <param name="configuration">Current configuration</param>
        /// <returns>Encrypted and signed string</returns>
        private static string EncryptAndSignCookie(string cookieValue, OAuthConfiguration configuration)
        {
            var encryptedCookie = 
                configuration.CryptographyConfiguration.EncryptionProvider.Encrypt(cookieValue);

            var hmacBytes = 
                GenerateHmac(encryptedCookie, configuration);

            var hmacString =
                Convert.ToBase64String(hmacBytes);

            return String.Format("{1}{0}", encryptedCookie, hmacString);
        }

        /// <summary>
        /// Generate a hmac for the encrypted cookie string
        /// </summary>
        /// <param name="encryptedCookie">Encrypted cookie string</param>
        /// <param name="configuration">Current configuration</param>
        /// <returns>Hmac byte array</returns>
        private static byte[] GenerateHmac(string encryptedCookie, OAuthConfiguration configuration)
        {
            return configuration.CryptographyConfiguration.HmacProvider.GenerateHmac(encryptedCookie);
        }

        /// <summary>
        /// Decrypt and validate an encrypted and signed cookie value
        /// </summary>
        /// <param name="cookieValue">Encrypted and signed cookie value</param>
        /// <param name="configuration">Current configuration</param>
        /// <returns>Decrypted value, or empty on error or if failed validation</returns>
        public static T DecryptAndValidateCookie<T>(Request request, OAuthConfiguration configuration)
        {
            var cookieValue =
                request.Cookies[OAuthAuthorizationCookieName];

            // TODO - shouldn't this be automatically decoded by nancy cookie when that change is made?
            var decodedCookie = Helpers.HttpUtility.UrlDecode(cookieValue);

            var hmacStringLength = Base64Helpers.GetBase64Length(configuration.CryptographyConfiguration.HmacProvider.HmacLength);

            var encryptedCookie = decodedCookie.Substring(hmacStringLength);
            var hmacString = decodedCookie.Substring(0, hmacStringLength);

            var encryptionProvider = configuration.CryptographyConfiguration.EncryptionProvider;

            // Check the hmacs, but don't early exit if they don't match
            var hmacBytes = Convert.FromBase64String(hmacString);
            var newHmac = GenerateHmac(encryptedCookie, configuration);
            var hmacValid = HmacComparer.Compare(newHmac, hmacBytes, configuration.CryptographyConfiguration.HmacProvider.HmacLength);

            var decrypted = 
                encryptionProvider.Decrypt(encryptedCookie);

            // Only return the decrypted result if the hmac was ok
            return hmacValid ? 
                Deserialize<T>(decrypted) : 
                default(T);
        }

        private static T Deserialize<T>(string value)
        {
            var serializer =
                new XmlSerializer(typeof (T));

            return (T) serializer.Deserialize(new StringReader(value));
        }

        private static string Serialize<T>(T instance)
        {
            var serializer =
                new XmlSerializer(typeof (T));

            var stream =
                new MemoryStream();

            serializer.Serialize(stream, instance);
            stream.Position = 0;

            using(var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }

    public interface IClient
    {
        string Id { get; set; }
        IEnumerable<string> Redirects { get; set; }
        string Secret { get; set; }
    }



    public interface IAuthorizationRequest
    {
        string ResponseType { get; set; } // REQUIRED. Should always be set to "code" as per the RFC
        string ClientId { get; set; } // REQUIRED
        Uri RedirectUri { get; set; } // OPTIONAL
        string Scope { get; set; } // OPTIONAL. List of space-delimited, case sensitive strings
        string State { get; set; } // OPTIONAL. An opaque value used by the client to maintain state between the request and callback. 
    }

    public interface IAuthorizationResponse
    {
        string Code { get; set; } // REQUIRED. The authorization code generated by the authorization server.
        string State { get; set; } // REQUIRED if the "state" parameter was present in the client authorization request. 
    }

    public interface IAccessTokenRequest
    {
        string ClientId { get; set; } // REQUIRED.  The client identifier issued to the client during the registration process
        string ClientSecret { get; set; } // REQUIRED.  The client secret
        string GrantType { get; set; } // REQUIRED.  Value MUST be set to "authorization_code".
        string Code { get; set; } // REQUIRED. The authorization code generated by the authorization server.
        Uri RedirectUri { get; set; } // REQUIRED, if the "redirect_uri" parameter was included in the authorization request
    }

    public interface IAccessTokenResponse
    {
        string AccessToken { get; set; } // REQUIRED.  The access token issued by the authorization server.
        string TokenType { get; set; } //  REQUIRED.  The type of the token issued
        int ExpiresIn { get; set; } // OPTIONAL.  The lifetime in seconds of the access token.
        string RefeshToken { get; set; } // OPTIONAL.  The refresh token which can be used to obtain new access tokens using the same authorization grant
        string Scope { get; set; } // OPTIONAL. List of space-delimited, case sensitive strings
    }

    public enum ErrorResponse
    {
    }

    public enum GrantType
    {
    }

    public enum TokenType
    {
    }
}
