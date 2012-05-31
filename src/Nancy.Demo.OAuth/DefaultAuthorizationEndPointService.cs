namespace Nancy.Demo.OAuth
{
    using System;
    using System.Linq;
    using Nancy.OAuth;

    public class DefaultAuthorizationEndPointService : IAuthorizationEndPointService
    {
        private readonly IApplicationStore applicationStore;

        public DefaultAuthorizationEndPointService(IApplicationStore applicationStore)
        {
            this.applicationStore = applicationStore;
        }

        public string GenerateAuthorizationToken(NancyContext context)
        {
            return string.Concat("authorization-token-", Guid.NewGuid().ToString());
        }

        public Tuple<string, object> GetAuthorizationView(AuthorizationRequest request, NancyContext context)
        {
            var application =
                this.applicationStore.First(app => app.Id.ToString().Equals(request.ClientId, StringComparison.OrdinalIgnoreCase));

            return new Tuple<string, object>(
                "authorize",
                new AuthorizeViewModel {
                    Body = "View model body",
                    Name = application.Name,
                    Description = application.Description,
                    Permissions = application.Permissions
                });
        }

        public AuthorizationRequestValidationResult ValidateRequest(AuthorizationRequest request, NancyContext context)
        {
            return new AuthorizationRequestValidationResult(AuthorizationErrorType.None);
        }
    }
}