namespace Nancy.Authentication.OAuth.Tests
{
    using System;
    using Bootstrapper;
    using FakeItEasy;
    using Helpers;
    using Nancy.Tests;
    using Testing;
    using Xunit;
    
    public class AuthorizationFixture
    {
        private readonly IViewModelDecorator viewModelDecorator;
        private readonly IApplicationRepository applicationRepository;
        private readonly IAuthorizationCodeGenerator authorizationCodeGenerator;
        private readonly ConfigurableBootstrapper bootstrapper;
        private readonly Browser browser;

        public AuthorizationFixture()
        {
            this.viewModelDecorator = A.Fake<IViewModelDecorator>();
            this.applicationRepository = A.Fake<IApplicationRepository>();
            this.authorizationCodeGenerator = A.Fake<IAuthorizationCodeGenerator>();

            this.bootstrapper = new ConfigurableBootstrapper(with =>
            {
                with.Module<AuthorizationCodeModule>();
                with.Dependency<IViewModelDecorator>(this.viewModelDecorator);
                with.Dependency<IApplicationRepository>(this.applicationRepository);
                with.Dependency<IAuthorizationCodeGenerator>(this.authorizationCodeGenerator);
            });

            OAuth.Enable(with => {
                with.Base = "/oauth";
                with.AuthorizationRequestRoute = "/authorize";
            });

            this.browser = new Browser(this.bootstrapper);
        }

        [Fact]
        public void Should_return_redirect_response_with_error_message_and_description_when_response_type_is_empty()
        {
            // Given
            var result = this.browser.Get("/oauth/authorize", with => {
                with.HttpsRequest();
                with.Query("Client_Id", "Nancy");
                with.Query("Redirect_Uri", "http://nancyfx.org");
                with.Query("Response_Type", string.Empty);
            });

            // When
            var location = new Uri(result.Context.Response.Headers["Location"]);
            var description = HttpUtility.UrlEncode("the request is missing a required parameter, includes an unsupported parameter or parameter value, or is otherwise malformed.");

            // Then
            result.StatusCode.ShouldEqual(HttpStatusCode.SeeOther);
            location.Query.ShouldContain("error=invalid_request");
            location.Query.ShouldContain(description);
        }

        [Fact]
        public void Should_return_redirect_response_with_error_message_and_description_when_response_type_is_not_code()
        {
            // Given, When
            var result = this.browser.Get("/oauth/authorize", with =>
            {
                with.HttpsRequest();
                with.Query("Client_Id", "Nancy");
                with.Query("Redirect_Uri", "http://nancyfx.org");
                with.Query("Response_Type", "foobar");
            });

            // When
            var location = new Uri(result.Context.Response.Headers["Location"]);
            var description = HttpUtility.UrlEncode("the request is missing a required parameter, includes an unsupported parameter or parameter value, or is otherwise malformed.");

            // Then
            result.StatusCode.ShouldEqual(HttpStatusCode.SeeOther);
            location.Query.ShouldContain("error=invalid_request");
            location.Query.ShouldContain(description);
        }

        [Fact]
        public void Should_return_redirect_response_with_error_message_and_description_when_client_id_is_empty()
        {
            // Given
            var result = this.browser.Get("/oauth/authorize", with =>
            {
                with.HttpsRequest();
                with.Query("Client_Id", string.Empty);
                with.Query("Redirect_Uri", "http://nancyfx.org");
                with.Query("Response_Type", "code");
            });

            // When
            var location = new Uri(result.Context.Response.Headers["Location"]);
            var description = HttpUtility.UrlEncode("the request is missing a required parameter, includes an unsupported parameter or parameter value, or is otherwise malformed.");

            // Then
            result.StatusCode.ShouldEqual(HttpStatusCode.SeeOther);
            location.Query.ShouldContain("error=invalid_request");
            location.Query.ShouldContain(description);
        }

        [Fact]
        public void Should_return_redirect_response_with_error_message_and_description_when_redirect_uri_is_empty()
        {
            // Given
            var result = this.browser.Get("/oauth/authorize", with =>
            {
                with.HttpsRequest();
                with.Query("Client_Id", "Nancy");
                with.Query("Redirect_Uri", string.Empty);
                with.Query("Response_Type", "code");
            });

            // When
            var location = new Uri(result.Context.Response.Headers["Location"]);
            var description = HttpUtility.UrlEncode("the request is missing a required parameter, includes an unsupported parameter or parameter value, or is otherwise malformed.");

            // Then
            result.StatusCode.ShouldEqual(HttpStatusCode.SeeOther);
            location.Query.ShouldContain("error=invalid_request");
            location.Query.ShouldContain(description);
        }
    }
}