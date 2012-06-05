namespace Nancy.OAuth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ModelBinding;

    public class AccessTokenRequest
    {
        public string GrantType { get; set; }

        public string Code { get; set; }

        public string RedirectUri { get; set; }

        public IEnumerable<string> Scope { get; set; }
    }

    public class AccessTokenRequestBinder : IModelBinder
    {
        /// <summary>
        /// Bind to the given model type
        /// </summary>
        /// <param name="context">Current context</param>
        /// <param name="modelType">Model type to bind to</param>
        /// <param name="blackList">Blacklisted property names</param>
        /// <returns>Bound model</returns>
        public object Bind(NancyContext context, Type modelType, params string[] blackList)
        {
            //return new AccessTokenRequest
            //{
            //    RedirectUri = context.Request.Query["redirect_uri"],
            //    GrantType = context.Request.Query["grant_type"],
            //    Code = context.Request.Query["code"]
            //};

            return new AccessTokenRequest
            {
                RedirectUri = "http://nancyfx.org",
                GrantType = "authorization_code",
                Code = context.Request.Query["code"],
                Scope = Enumerable.Empty<string>()
            };
        }

        /// <summary>
        /// Whether the binder can bind to the given model type
        /// </summary>
        /// <param name="modelType">Required model type</param>
        /// <returns>True if binding is possible, false otherwise</returns>
        public bool CanBind(Type modelType)
        {
            return modelType.Equals(typeof(AccessTokenRequest));
        }
    }
}