namespace Nancy.Authentication.OAuth
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
                .Select(x => string.Concat(x.Name, "=", HttpUtility.UrlEncode(x.GetValue(source, null).ToString().ToLower()))); // Should do ToLower once the binder is sorted out

            return string.Concat("?", string.Join("&", keyValuePairs));
        }
    }
}