using System.IO;
using System.Net;
using System.Text;

namespace Nancy.Demo.Authentication.OAuth
{
    using System;
    using System.Dynamic;
    using Extensions;
    using Helpers;
    using ModelBinding;
    using Nancy.Authentication.Forms;
    using Nancy.Authentication.OAuth;

    public class RootModule : NancyModule
    {
        public RootModule()
        {
            Get["/"] = parameters => {
                
                var returnUrl =
                    HttpUtility.UrlEncode(Context.ToFullPath("~/authreturn"));

                var path =
                    "~/oauth/authorize?Client_Id=NancyApp&&Response_Type=code&Redirect_Uri=" + returnUrl + "&state=ApplicationStateValue";

                return "<a href='" + Context.ToFullPath(path) + "'>Authorize</a>";
            };

            Get["/logout"] = x => {
                return this.LogoutAndRedirect("~/");
            };

            Get["/login"] = x => {
                dynamic model = new ExpandoObject();
                model.Errored = this.Request.Query.error.HasValue;

                return View["login", model];
            };

            Post["/login"] = x => {
                var userGuid = UserDatabase.ValidateUser((string)this.Request.Form.Username, (string)this.Request.Form.Password);

                if (userGuid == null)
                {
                    return Context.GetRedirect("~/login?error=true&username=" + (string)this.Request.Form.Username);
                }

                DateTime? expiry = null;
                if (this.Request.Form.RememberMe.HasValue)
                {
                    expiry = DateTime.Now.AddDays(7);
                }

                return this.LoginAndRedirect(userGuid.Value, expiry);
            };
        }
    }

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

                //var targetUrl = string.Concat(
                //    Context.ToFullPath("~" + OAuth.Configuration.Base + OAuth.Configuration.AuthenticationRoute),
                //    "?code=",
                //    (string)Request.Query.code,
                //    "&Client_Id=",
                //    "NancyApp",
                //    "&Client_Secret=Secret",
                //    "&Redirect_Uri=",
                //    HttpUtility.UrlEncode(Context.ToFullPath("~/oauthtokenresponse")));

                //return "Returned from authentication with code [" + Request.Query.code + "] and state [" + Request.Query.state + "]";

                var request = new AccessTokenRequest
                {
                    Client_Id = "NancyApp",
                    Client_Secret = "Secret",
                    Code = (string)Request.Query.Code,
                    Grant_Type = "authentication",
                    Redirect_Uri = HttpUtility.UrlEncode(Context.ToFullPath("~/oauthtokenresponse"))
                };

                //var targetUrl = string.Concat(
                //    Context.ToFullPath("~" + OAuth.Configuration.Base + OAuth.Configuration.AuthenticationRoute),
                //    request.AsQueryString());

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

                //return Response.AsRedirect(targetUrl);
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