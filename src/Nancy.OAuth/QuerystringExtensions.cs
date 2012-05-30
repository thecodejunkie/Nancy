namespace Nancy.OAuth
{
    using System.Linq;
    using System.Reflection;
    using Helpers;

    public static class QuerystringExtensions
    {
        public static string AsQueryString(this object source)
        {
            var keyValuePairs = source
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetValue(source, null) != null)
                .Select(x => string.Concat(x.Name, "=", HttpUtility.UrlEncode((string) x.GetValue(source, null).ToString())));

            return string.Concat("?", string.Join("&", keyValuePairs));
        }
    }
}