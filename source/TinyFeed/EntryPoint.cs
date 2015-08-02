using System;
using Autofac;
using Microsoft.Owin.Hosting;
using Owin;
using Topshelf;

namespace TinyFeed
{
    public class EntryPoint
    {
        private readonly object startStopLock = new object();

        private CustomStartup startup;
        private IDisposable webapp;

        public static void Main()
        {
            HostFactory.Run(x =>
            {
                x.SetServiceName("TinyFeed");
                x.SetDescription("TinyFeed");
                x.SetDisplayName("TinyFeed");
                x.Service<EntryPoint>(c =>
                {
                    c.ConstructUsing(f => new EntryPoint());
                    c.WhenStarted(e => e.Start());
                    c.WhenStopped(e => e.Stop());
                });
            });
        }

        private void Start()
        {
            lock (startStopLock)
            {
                webapp = WebApp.Start("http://*:9002/", WebAppStartup);
            }
        }

        private void Stop()
        {
            lock (startStopLock)
            {
                if (webapp != null)
                {
                    webapp.Dispose();
                    webapp = null;
                }

                if (startup != null)
                {
                    startup.WaitForShutdown(TimeSpan.FromSeconds(5));
                    startup = null;
                }
            }
        }

        protected virtual void WebAppStartup(IAppBuilder app)
        {
            startup = new CustomStartup(this);
            startup.Configuration(app);
        }

        protected virtual INuGetWebApiSettings CreateSettings()
        {
            return new NuGetWebApiSettings();
        }

        protected virtual IContainer CreateContainer(IAppBuilder app)
        {
            return startup.CreateDefaultContainer(app);
        }

        private class CustomStartup : Startup
        {
            private readonly EntryPoint entryPoint;

            public CustomStartup(EntryPoint entryPoint)
            {
                this.entryPoint = entryPoint;
            }

            protected override INuGetWebApiSettings CreateSettings()
            {
                return entryPoint.CreateSettings();
            }

            public IContainer CreateDefaultContainer(IAppBuilder app)
            {
                return base.CreateContainer(app);
            }

            protected override IContainer CreateContainer(IAppBuilder app)
            {
                return entryPoint.CreateContainer(app);
            }
        }
    }
}