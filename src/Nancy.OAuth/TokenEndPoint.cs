namespace Nancy.OAuth
{
    using System.Collections.Generic;
    using ModelBinding;

    public class TokenEndPoint : NancyModule
    {
        public TokenEndPoint(ITokenEndPointService service) : base("/oauth/token")
        {
            Get["/"] = parameters => {
                var request =
                    this.Bind<TokenRequest>();

                return 200;
            };
        }
    }

    public interface ITokenEndPointService
    {
    }

    public class DefaultTokenEndPointService : ITokenEndPointService
    {
    }

    public class TokenRequest
    {
    }
}