namespace Nancy.OAuth
{
    using Nancy.Bootstrapper;
    using Session;

    /// <summary>
    /// Just a quick implementation until the session/cache provided changes are in Master
    /// </summary>
    public class InMemorySessions : IObjectSerializerSelector
    {
        private static ISession session = new Session();

        public static void Enable(IPipelines pipelines)
        {
            pipelines.BeforeRequest.AddItemToStartOfPipeline(ctx => {
                if(ctx.Request != null)
                {
                    ctx.Request.Session = session;
                }

                return null;
            });

            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>{
                if (ctx.Request.Session != null || ctx.Request.Session.HasChanged)
                {
                    session = ctx.Request.Session;
                }
            });
        }

        public void WithSerializer(IObjectSerializer newSerializer)
        {
        }
    }
}