namespace Nancy.Routing
{
    using System;

    /// <summary>
    /// Defines the functionality for retrieving metadata for routes.
    /// </summary>
    /// <typeparam name="TMetadata">The metadata type.</typeparam>
    public abstract class RouteMetadataProvider<TMetadata> : IRouteMetadataProvider
    {
        public Type MetadataType
        {
            get { return typeof(TMetadata); }
        }

        public object GetMetadata(INancyModule module, RouteDescription routeDescription)
        {
            return this.GetRouteMetadata(module, routeDescription);
        }

        /// <summary>
        /// Gets the metadata for the provided route.
        /// </summary>
        /// <param name="module">The <see cref="INancyModule"/> instance that the route is declared in.</param>
        /// <param name="routeDescription">A <see cref="RouteDescription"/> for the route.</param>
        /// <returns>An instance of <see cref="TMetadata"/>.</returns>
        protected abstract TMetadata GetRouteMetadata(INancyModule module, RouteDescription routeDescription);
    }
}