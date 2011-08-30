namespace Nancy.Authentication.OAuth
{
    using System;
    using Xunit;

    public class Authorization
    {
        public string Client_Id { get; set; }
        
        public string Redirect_Uri { get; set; }

        public string Response_Type { get; set; }
        
        public string Scope { get; set; }
        
        public string State { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(this.Client_Id) &&
                   !string.IsNullOrEmpty(this.Redirect_Uri) &&
                   !string.IsNullOrEmpty(this.Response_Type) &&
                   this.Response_Type.Equals("code", StringComparison.OrdinalIgnoreCase);
        }
    }

    public class AuthorizationErrorResponse
    {
        public string Error { get; set; }

        public string Error_Description { get; set; }

        public string State { get; set; }
    }

    public class Foo
    {
        [Fact]
        public void Test()
        {
            var bar = new AuthorizationErrorResponse {
                Error = "unsupported_response_type",
                Error_Description = "The authorization server does not support obtaining an authorization code using this method."
            };

            var querystring = bar.AsQueryString();

            Assert.Equal(querystring, string.Empty);
        }
    }
}