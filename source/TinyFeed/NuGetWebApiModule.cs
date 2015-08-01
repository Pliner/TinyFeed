using Autofac;
using Autofac.Integration.WebApi;
using TinyFeed.Core;

namespace TinyFeed
{
    public class NuGetWebApiModule : Module
    {
        private readonly INuGetWebApiSettings settings;

        public NuGetWebApiModule()
            : this(new NuGetWebApiSettings())
        {
        }

        public NuGetWebApiModule(INuGetWebApiSettings settings)
        {
            this.settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(settings).As<INuGetWebApiSettings>();

            var routeMapper = new NuGetWebApiRouteMapper(settings.RoutePathPrefix);
            builder.RegisterInstance(routeMapper);

            builder.Register(x => new TinyFeedContext()).As<ITinyFeedContext>().InstancePerRequest();
            builder.Register(x => new TinyFeedPackageService(x.Resolve<ITinyFeedContext>())).As<ITinyFeedPackageService>();

            builder.RegisterApiControllers(typeof (NuGetWebApiModule).Assembly);
        }
    }
}