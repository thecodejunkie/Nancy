namespace Nancy.Authentication.OAuth
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Reflection;

    public static class ResponseFormatterExtensions
    {
        public static Response AsErrorResponse(this IResponseFormatter source, AuthorizationErrorResponse error, string redirectUri)
        {
            return source.AsRedirect(string.Concat(redirectUri, error.AsQueryString()));
        }

        public static Response AsStatusCode(this IResponseFormatter source, HttpStatusCode statusCode)
        {
            return statusCode;
        }
    }

    public static class ModelExtensions
    {
        private static bool IgnoreNullableWithValue(PropertyInfo info, object source)
        {
            return !(Nullable.GetUnderlyingType(info.PropertyType) != null) && (info.GetValue(source, null) != null);
        }

        public static dynamic AsExpandoObject(this object source, bool ignorePropertiesWithDefaultValue = true)
        {
            var properties = source
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .AsEnumerable();

            if (ignorePropertiesWithDefaultValue)
            {
                properties = properties
                    .Where(x => IgnoreNullableWithValue(x, source))
                    .Where(x => x.GetValue(source, null) != null);
            }
                

            var expando = new ExpandoObject();
            var expandoDictionary = (IDictionary<string, object>)expando;

            foreach (var property in properties)
            {
                expandoDictionary[property.Name] = property.GetValue(source, null);
            }

            return expandoDictionary;
        }
    }
}