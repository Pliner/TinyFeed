using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData.Extensions;
using System.Web.Http.OData.Formatter;
using System.Web.Http.OData.Formatter.Deserialization;
using System.Web.Http.OData.Routing;
using System.Web.Http.OData.Routing.Conventions;
using System.Web.Http.Routing;
using TinyFeed.OData.Batch;
using TinyFeed.OData.Formatter.Serialization;
using TinyFeed.OData.Routing.Conventions;

namespace TinyFeed
{
    public class NuGetWebApiRouteMapper
    {
        private readonly string pathPrefix;

        public NuGetWebApiRouteMapper(string pathPrefix)
        {
            this.pathPrefix = pathPrefix;
        }

        /// <summary>
        /// Adds route handlers that will redirect NuGet User Agents so that
        /// a single source URL can be used in the client.
        /// </summary>
        /// <param name="config">
        /// The configuration in which to register routes
        /// </param>
        public void MapNuGetClientRedirectRoutes(HttpConfiguration config)
        {
            // redirect NuGet user agents from / to /api/odata
            config.Routes.MapHttpRoute(RouteNames.Redirect.RootFeed,
                                string.Empty,
                                new { },
                                new { userAgent = new NuGetUserAgentConstraint() },
                                new ODataRedirectHandler(RouteNames.Packages.Feed));

            // redirect NuGet user agents from /api to /api/odata
            config.Routes.MapHttpRoute(RouteNames.Redirect.ApiFeed,
                                pathPrefix,
                                new { },
                                new { userAgent = new NuGetUserAgentConstraint() },
                                new ODataRedirectHandler(RouteNames.Packages.Feed));

            // redirect e.g. PUT /api/odata to PUT /api/packages
            config.Routes.MapHttpRoute(RouteNames.Redirect.Upload,
                                ODataRoutePath,
                                new { },
                                new { method = new HttpMethodConstraint(HttpMethod.Put) },
                                new RedirectHandler(RouteNames.Packages.Upload));
        }

        public void MapApiRoutes(HttpConfiguration config)
        {
            var routes = config.Routes;

            routes.MapHttpRoute(RouteNames.Packages.GetAvailableSearchFieldNames,
                                pathPrefix + "packages/$searchable-fields",
                                new { controller = "Packages", action = "GetAvailableSearchFieldNames" },
                                new { httpMethod = new HttpMethodConstraint(HttpMethod.Get, HttpMethod.Options) });

            routes.MapHttpRoute(RouteNames.Packages.Upload,
                                pathPrefix + "packages",
                                new { controller = "Packages" },
                                new { httpMethod = new HttpMethodConstraint(HttpMethod.Put, HttpMethod.Options) });

            routes.MapHttpRoute(RouteNames.Packages.DownloadLatestVersion,
                                pathPrefix + "packages/{id}/content",
                                new { controller = "Packages", action = "DownloadPackage" });

            routes.MapHttpRoute(RouteNames.Packages.Download,
                                pathPrefix + "packages/{id}/{version}/content",
                                new { controller = "Packages", action = "DownloadPackage" },
                                new { version = new SemanticVersionConstraint() });
        }

        public void MapODataRoutes(HttpConfiguration config)
        {
            var builder = new NuGetWebApiODataModelBuilder();

            builder.Build();

            config.Formatters.InsertRange(0,
                ODataMediaTypeFormatters.Create(
                    new ODataPackageDefaultStreamAwareSerializerProvider(),
                    new DefaultODataDeserializerProvider()));

            var conventions = new List<IODataRoutingConvention>
            {
                new CompositeKeyRoutingConvention(),
                new CompositeKeyPropertyRoutingConvention(),
                new NonBindableActionRoutingConvention("PackagesOData"),
            };

            conventions.AddRange(ODataRoutingConventions.CreateDefault());

            conventions = conventions.Select(c => new ControllerAliasingODataRoutingConvention(c, "Packages", "PackagesOData")).Cast<IODataRoutingConvention>().ToList();

            config.Routes.MapODataServiceRoute(
                RouteNames.Packages.Feed,
                ODataRoutePath,
                builder.Model,
                new DefaultODataPathHandler(),
                conventions,
                new HeaderCascadingODataBatchHandler(new BatchHttpServer(config)));
        }

        public string PathPrefix { get { return pathPrefix; } }
        public string ODataRoutePath { get { return PathPrefix + "odata"; } }

        /// <summary>
        /// See https://trocolate.wordpress.com/2012/07/19/mitigate-issue-260-in-batching-scenario/
        /// </summary>
        public class BatchHttpServer : HttpServer
        {
            private readonly HttpConfiguration config;

            public BatchHttpServer(HttpConfiguration config)
                : base(config)
            {
                this.config = config;
            }

            protected override void Initialize()
            {
                var firstInPipeline = config.MessageHandlers.FirstOrDefault();
                if (firstInPipeline != null && firstInPipeline.InnerHandler != null)
                {
                    InnerHandler = firstInPipeline;
                }
                else
                {
                    base.Initialize();
                }
            }
        }
    }
}
