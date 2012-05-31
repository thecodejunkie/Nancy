namespace Nancy.Demo.OAuth
{
    using System.Linq;
    using ModelBinding;
    using Security;

    public class ApplicationsModule : NancyModule
    {
        public ApplicationsModule(IApplicationStore store, IApplicationFactory factory) : base("/applications")
        {
            this.RequiresAuthentication();

            Get["/create"] = parameters => {
                return View["create"];
            };

            Post["/create"] = parameters => {
                var model = 
                    this.Bind<ApplicationModel>();

                var application =
                    factory.Create(model);

                store.Add(application);

                return Response.AsRedirect("~/applications/display/" + application.Id.ToString());
            };

            Get["/display/{id}"] = parameters => {
                return View["display", store.First(x => x.Id.ToString().Equals(parameters.id))];
            };

            Get["/list"] = parameters => {
                return View["list", store];
            };
        }
    }
}