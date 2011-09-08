namespace Nancy.Demo.Authentication.OAuth
{
    using System.IO;
    using System.Net;
    using System.Text;
    using Extensions;
    using Helpers;
    using ModelBinding;
    using Nancy.Authentication.OAuth;

    /// <summary>
    /// Simulates a client application that is requesting OAuath approval
    /// </summary>
    public class ClientModule : NancyModule
    {
        public ClientModule()
        {
            Get["/authreturn"] = parameters =>
            {
                if (Request.Query.error.HasValue)
                {
                    return string.Concat(Request.Query.error, " - ", Request.Query["error_description"]);
                }

                var request = new AccessTokenRequest
                {
                    Client_Id = "NancyApp",
                    Client_Secret = "Secret",
                    Code = (string)Request.Query.Code,
                    Grant_Type = "authentication",
                    Redirect_Uri = HttpUtility.UrlEncode(Context.ToFullPath("~/oauthtokenresponse"))
                };

                var targetUrl = string.Concat(
                    "http://localhost:60644",
                    OAuth.Configuration.Base,
                    OAuth.Configuration.AuthenticationRoute);

                var response = HttpPost(
                    targetUrl,
                    request.AsQueryString().Substring(1),
                    this.Context);

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return "The access token endpoint replied with : " + reader.ReadToEnd();
                }
            };

            Get["/oauthtokenresponse"] = parameters =>
            {
                // This should be removed
                var token = 
                    this.Bind<AccessTokenResponse>();

                return "Returned with oauth_token " + token.Access_Token;
            };
        }

        public static WebResponse HttpPost(string uri, string parameters, NancyContext context)
        {
            var req = 
                (HttpWebRequest)WebRequest.Create(uri);

            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";

            var bytes = 
                Encoding.ASCII.GetBytes(parameters);
            req.ContentLength = bytes.Length;
            
            var os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length); //Push it out there
            os.Close();

            return req.GetResponse();
        }
    }
}