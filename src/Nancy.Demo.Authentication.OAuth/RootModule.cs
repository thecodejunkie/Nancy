namespace Nancy.Demo.Authentication.OAuth
{
    using System;
    using System.Dynamic;
    using Extensions;
    using Helpers;
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
                    "~/oauth/authorize?Client_Id=NancyApp&Redirect_Uri=" + returnUrl + "&state=ApplicationStateValue";

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

            Get["/authreturn"] = parameters => {

                var targetUrl = string.Concat(
                    Context.ToFullPath("~" + OAuth.Configuration.Base + OAuth.Configuration.AuthenticationRoute),
                    "?code=",
                    (string)Request.Query.code,
                    "&Client_Id=",
                    "NancyApp",
                    "&Client_Secret=Secret",
                    "&Redirect_Uri=",
                    HttpUtility.UrlEncode(Context.ToFullPath("~/oauthtokenresponse")));

                //return "Returned from authentication with code [" + Request.Query.code + "] and state [" + Request.Query.state + "]";

                return Response.AsRedirect(targetUrl);
            };

            Get["/oauthtokenresponse"] = parameters => {
                return "Returned with oauth_token " + Request.Query["oauth_token"];
            };
        }
    }
}