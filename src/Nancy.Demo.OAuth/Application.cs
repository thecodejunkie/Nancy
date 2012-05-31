namespace Nancy.Demo.OAuth
{
    using System;
    using System.Collections.Generic;

    public class Application
    {
        public Application(ApplicationModel model, IEnumerable<string> permissions)
            : this(Guid.NewGuid(), model.Name, model.Description, model.Website, model.Callback, permissions)
        {
        }

        public Application(Guid id, string name, string description, Uri website, Uri callback, IEnumerable<string> permissions)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Website = website;
            this.Callback = callback;
            this.Permissions = permissions;
        }

        public Guid Id { get; private set; }

        public Uri Callback { get; set; }

        public string Description { get; private set; }

        public string Name { get; private set; }

        public IEnumerable<string> Permissions { get; set; }

        public Uri Website { get; private set; }
    }
}