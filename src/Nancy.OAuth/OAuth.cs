namespace Nancy.OAuth
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Security;

    public static class OAuth
    {
        /// <summary>
        /// The configuration used by the OAuth provider.
        /// </summary>
        public static OAuthConfiguration Configuration = new OAuthConfiguration();

        public static bool IsEnabled { get; private set; }

        public static void Enable()
        {
            IsEnabled = true;
        }

        public static void Enable(Action<OAuthConfiguration> closure)
        {
            var configuration =
                new OAuthConfiguration();

            closure.Invoke(configuration);

            Enable(configuration);
        }

        public static void Enable(OAuthConfiguration configuration)
        {
            Configuration = configuration;
            IsEnabled = true;
        }

        public class OAuthConfiguration
        {
            /// <summary>
            /// 
            /// </summary>
            public OAuthConfiguration()
            {
                this.AuthorizationRequestRoute = "/authorize";
                this.AuthorizationAllowRoute = "/allow";
                this.AuthorizationDenyRoute = "/deny";
                this.Base = "/oauth";
            }

            public string Base { get; set; }

            public string AuthorizationRequestRoute { get; set; }

            public string AuthorizationAllowRoute { get; set; }

            public string AuthorizationDenyRoute { get; set; }

            public string GetFullPath(Expression<Func<OAuthConfiguration, object>> expression)
            {
                var member =
                    expression.GetTargetMemberInfo() as PropertyInfo;

                if (member == null)
                {
                    throw new InvalidOperationException();
                }

                var value =
                    member.GetValue(this, null);

                return string.Concat(this.Base, "/", value) ;
            }
        }
    }

     public static class ExpressionExtensions
    {
        /// <summary>
        /// Retrieves the member that an expression is defined for.
        /// </summary>
        /// <param name="expression">The expression to retreive the member from.</param>
        /// <returns>A <see cref="MemberInfo"/> instance if the member could be found; otherwise <see langword="null"/>.</returns>
        public static MemberInfo GetTargetMemberInfo(this Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Convert:
                    return GetTargetMemberInfo(((UnaryExpression)expression).Operand);
                case ExpressionType.Lambda:
                    return GetTargetMemberInfo(((LambdaExpression)expression).Body);
                case ExpressionType.Call:
                    return ((MethodCallExpression)expression).Method;
                case ExpressionType.MemberAccess:
                    return ((MemberExpression)expression).Member;
                default:
                    return null;
            }
        }
    }
}