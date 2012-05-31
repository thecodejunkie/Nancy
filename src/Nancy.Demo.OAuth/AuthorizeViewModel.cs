namespace Nancy.Demo.OAuth
{
    using System.Collections.Generic;

    public class AuthorizeViewModel
    {
        public string Body { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> Permissions { get; set; }
    }
}