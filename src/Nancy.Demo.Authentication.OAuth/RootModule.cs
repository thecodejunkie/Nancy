namespace Nancy.Demo.Authentication.OAuth
{
    using System;
    using System.Dynamic;
    using Extensions;
    using Nancy.Authentication.Forms;

    public class RootModule : NancyModule
    {
        public RootModule()
        {
            Get["/"] = parameters =>
            {
                return "<a href='" + Context.ToFullPath("~/oauth/authorize?Client_Id=NancyApp&Redirect_Uri=http%3A%2F%2Fwww.google.com&state=ApplicationStateValue") + "'>Authorize</a>";
            };

            Get["/logout"] = x =>
            {
                return this.LogoutAndRedirect("~/");
            };

            Get["/login"] = x =>
            {
                dynamic model = new ExpandoObject();
                model.Errored = this.Request.Query.error.HasValue;

                return View["login", model];
            };

            Post["/login"] = x =>
            {
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