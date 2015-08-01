using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ExceptionHandling;
using AspNet.WebApi.HtmlMicrodataFormatter;
using Autofac;
using Microsoft.Owin.Diagnostics;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Owin;
using TinyFeed.Core;
using TinyFeed.Filters;
using TinyFeed.MessageHandlers;

namespace TinyFeed
{
    public class Startup
    {
        private readonly ManualResetEventSlim shutdownSignal = new ManualResetEventSlim(false);

        private INuGetWebApiSettings Settings { get; set; }

        public void Configuration(IAppBuilder app)
        {
            SignatureConversions.AddConversions(app);
            Settings = CreateSettings();
            Start(app, CreateContainer(app));
        }

        public bool WaitForShutdown(TimeSpan timeout)
        {
            return shutdownSignal.Wait(timeout);
        }

        protected virtual INuGetWebApiSettings CreateSettings()
        {
            return new NuGetWebApiSettings();
        }

        protected virtual void Start(IAppBuilder app, IContainer container)
        {
            var config = CreateHttpConfiguration();

            ConfigureWebApi(config);

            if (Settings.ShowExceptionDetails)
            {
                app.UseErrorPage(new ErrorPageOptions
                {
                    ShowExceptionDetails = true,
                    ShowSourceCode = true
                });
            }

            app.UseAutofacMiddleware(container);
            app.UseStageMarker(PipelineStage.Authenticate);

            app.UseAutofacWebApi(config);
            app.UseWebApi(config);

            app.UseStageMarker(PipelineStage.MapHandler);

            RegisterServices(container, app, config);

            CreateScheme();
        }

        private static void CreateScheme()
        {
            using (var context = new TinyFeedContext())
            {
                context.Initialize();
            }
        }

        protected virtual HttpConfiguration CreateHttpConfiguration()
        {
            return new HttpConfiguration();
        }

        protected virtual IContainer CreateContainer(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new NuGetWebApiModule(Settings));

            return builder.Build();
        }

        protected virtual void RegisterServices(IContainer container, IAppBuilder app, HttpConfiguration config)
        {
            var apiMapper = container.Resolve<NuGetWebApiRouteMapper>();

            apiMapper.MapNuGetClientRedirectRoutes(config);
            apiMapper.MapApiRoutes(config);
            apiMapper.MapODataRoutes(config);
        }

        protected virtual void ConfigureWebApi(HttpConfiguration config)
        {
            config.IncludeErrorDetailPolicy = Settings.ShowExceptionDetails
                ? IncludeErrorDetailPolicy.Always
                : IncludeErrorDetailPolicy.Default;

            config.MessageHandlers.Add(new CrossOriginMessageHandler(Settings.EnableCrossDomainRequests));
            config.Filters.Add(new ExceptionLoggingFilter());

            var documentation = new HtmlDocumentation();
            documentation.Load();

            config.Services.Replace(typeof(IDocumentationProvider), new WebApiHtmlDocumentationProvider(documentation));
            config.Services.Replace(typeof(IExceptionHandler), new LoggingExceptionHandler());

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
        }

        private class LoggingExceptionHandler : IExceptionHandler
        {
            private static readonly Task CompletedTask;

            static LoggingExceptionHandler()
            {
                var tcs = new TaskCompletionSource<bool>();
                tcs.SetResult(true);
                CompletedTask = tcs.Task;
            }

            public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
            {
                UnhandledExceptionLogger.LogException(context.Exception);
                return CompletedTask;
            }
        }
    }
}
