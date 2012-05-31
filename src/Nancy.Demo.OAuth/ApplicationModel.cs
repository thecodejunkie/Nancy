namespace Nancy.Demo.OAuth
{
    using System;

    public class ApplicationModel
    {
        public Uri Callback { get; private set; }

        public string Description { get; private set; }

        public string Name { get; private set; }

        public Uri Website { get; private set; }
    }
}