namespace Nancy.Demo.OAuth
{
    using System;
    using Nancy.OAuth;

    public class DefaultAccessTokenEndPointService : IAccessTokenEndPointService
    {
        public AccessTokenResponse CreateAccessTokenResponse(AccessTokenRequest tokenRequest, NancyContext context)
        {
            return new AccessTokenResponse
            {
                Access_Token = string.Concat("access-token-", Guid.NewGuid().ToString("D"))
            };
        }

        public ValidationResult ValidateRequest(AccessTokenRequest tokenRequest, NancyContext context)
        {
            return new ValidationResult(ErrorType.None);
        }
    }
}