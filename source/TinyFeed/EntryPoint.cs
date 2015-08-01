using System;
using Autofac;
using Common.Logging;
using Microsoft.Owin.Hosting;
using Owin;

namespace TinyFeed
{
    public class EntryPoint : IDisposable
    {
        private CustomStartup startup;
        private IDisposable webapp;

        static void Main()
        {
            var log = LogManager.GetCurrentClassLogger();

            try
            {
                using (var program = new EntryPoint())
                {
                    program.Start();
                    Console.WriteLine("Listening on " + program.BaseAddress + ". Press <ctrl>+c to stop listening.");
                    Console.WriteLine("Press enter to stop.");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                log.Fatal(m => m(ex.Message), ex);
            }

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

        public void Start(string baseAddress = "http://*:9002/")
        {
            BaseAddress = baseAddress;
            webapp = WebApp.Start(baseAddress, WebAppStartup);
        }

        public string BaseAddress { get; private set; }

        public void Dispose()
        {
            if (webapp != null)
            {
                webapp.Dispose();
                webapp = null;
            }

            if (startup != null)
            {
                startup.WaitForShutdown(TimeSpan.FromMinutes(1));
                startup = null;
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

        class CustomStartup : Startup
        {
            readonly EntryPoint entryPoint;

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