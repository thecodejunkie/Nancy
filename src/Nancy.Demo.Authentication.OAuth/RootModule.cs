namespace Nancy.Demo.Authentication.OAuth
{
    using System;
    using System.Dynamic;
    using Extensions;
    using Helpers;
    using Nancy.Authentication.Forms;

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
}