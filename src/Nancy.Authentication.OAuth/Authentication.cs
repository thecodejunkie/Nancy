namespace Nancy.Authentication.OAuth
{
    using System;

    public class Authentication
    {
        public string Client_Id { get; set; }

        public string Client_Secret { get; set; }

        public string Code { set; get; }

        public string Grant_Type { get; set; }

        public string Redirect_Uri { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(this.Client_Id) &&
                   !string.IsNullOrEmpty(this.Client_Secret) &&
                   !string.IsNullOrEmpty(this.Code) &&
                   !string.IsNullOrEmpty(this.Grant_Type) &&
                   this.Grant_Type.Equals("authorization_code", StringComparison.OrdinalIgnoreCase) &&
                   !string.IsNullOrEmpty(this.Redirect_Uri); // TODO: This is not mandatory according to 4.1.3
        }
    }
}