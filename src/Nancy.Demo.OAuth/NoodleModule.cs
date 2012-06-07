namespace Nancy.Demo.OAuth
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using ModelBinding;
    using Nancy;
    using Security;

    public class NoodleModule : NancyModule
    {
        public NoodleModule(INoodleService service) : base("/noodle")
        {
            Get["/"] = parameters =>
            {
                return View["noodle/index", service];
            };

            Post["/"] = parameters =>
            {
                var model =
                    this.Bind<NoodleModel>(new[] { "Posted" });

                service.Add(model);

                return Response.AsRedirect("~/noodle");
            };
        }
    }

    public class NoodleApiModule : NancyModule
    {
        public NoodleApiModule(INoodleService service) : base("/noodle/api")
        {
            this.RequiresAuthentication();

            Post["/"] = parameters => {
                service.Add(new NoodleModel
                {
                    Author = string.Concat(Context.CurrentUser.UserName, " (from API)"),
                    Message = "This message was automatically generated when posting from the API"
                });

                return 200;
            };
        }
    }

    public class NoodleModel
    {
        public NoodleModel()
        {
            this.Posted = DateTime.Now;
        }

        public string Author { get; set; }

        public string Message { get; set; }

        public DateTime Posted { get; set; }
    }

    public interface INoodleService : IEnumerable<NoodleModel>
    {
        void Add(NoodleModel message);
    }

    public class InMemoryNoodleService : INoodleService
    {
        private readonly IList<NoodleModel> cache;

        public InMemoryNoodleService()
        {
            this.cache = new List<NoodleModel>();
        }

        public IEnumerator<NoodleModel> GetEnumerator()
        {
            return this.cache.GetEnumerator();
        }

        public void Add(NoodleModel message)
        {
            this.cache.Add(message);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}