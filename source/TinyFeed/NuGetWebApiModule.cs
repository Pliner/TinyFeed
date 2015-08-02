using System;
using System.Configuration;
using System.IO;
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

            builder.Register(x => new DateTimeService()).As<IDateTimeService>();
            builder.Register(x => new CryptoService()).As<ICryptoService>();
            builder.Register(x => new Context()).As<IContext>().InstancePerRequest();
            builder.Register(x => new PackageBuilder(x.Resolve<ICryptoService>(), x.Resolve<IDateTimeService>())).As<IPackageBuilder>();
            builder.Register(x => new PackageService(x.Resolve<IContext>())).As<IPackageService>();
            builder.Register(x => new BlobService(GetBlobPath())).As<IBlobService>();

            builder.RegisterApiControllers(typeof (NuGetWebApiModule).Assembly);
        }

        private static string GetBlobPath()
        {
            var packageDirectory = ConfigurationManager.AppSettings["PackagesDirectory"];
            if (Path.IsPathRooted(packageDirectory))
                return packageDirectory;
            var baseDirectory = GetBaseDirectory();
            return Path.Combine(baseDirectory, packageDirectory);
        }

        private static string GetBaseDirectory()
        {
            return Path.GetFullPath(string.IsNullOrEmpty(AppDomain.CurrentDomain.RelativeSearchPath)
                ? AppDomain.CurrentDomain.BaseDirectory
                : AppDomain.CurrentDomain.RelativeSearchPath
                );
        }
    }
}