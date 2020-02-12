using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace TNDStudios.AppMonitor.Core
{
    public static class SetupExtensions
    {
        /// <summary>
        /// Reference to the services provider built from the service collection used on startup so it can be used in building the app
        /// and resolving service instances
        /// </summary>
        private static IServiceCollection services;

        /// <summary>
        /// Extension of the Service Collection so it can be added in Web Application startup
        /// </summary>
        /// <param name="serviceCollection">The Sercice Collection injected into the Web Application</param>
        /// <returns>The modified Service Collection</returns>
        public static IServiceCollection AddAppMonitor(this IServiceCollection serviceCollection,
            IAppMonitorConfig configuration)
        {
            // Parse and clean the configuration
            configuration.Parse();

            // Add a singleton for the App Monitor Core and the configuration so it can be injected in to constructors etc.
            serviceCollection.AddSingleton<IAppMonitorCoordinator>(new AppMonitorCoordinator() { });
            serviceCollection.AddSingleton<IAppMonitorConfig>(configuration);

            // Make sure SignalR is added as a service
            serviceCollection.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });

            serviceCollection.AddControllers();
            serviceCollection.AddRouting();

            serviceCollection.AddHostedService<MetricMonitor>();

            // Assign locally to be picked up by the UseAppMonitor method to create the serviceProvider implementation
            services = serviceCollection; 

            return serviceCollection;
        }

        /// <summary>
        /// Extension of the application builder so that the Web Application startup
        /// </summary>
        /// <param name="applicationBuilder">The Application Builder injected into the Web Application</param>
        /// <returns>The modified Application Builder</returns>
        public static IApplicationBuilder UseAppMonitor(this IApplicationBuilder app)
        {
            // Enforce Routing Usage
            app.UseRouting();

            // Remind the system of the services reference for use later to resolve instances (Singletons)
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Get instance of the configuration from the service provider
            IAppMonitorConfig configuration = (IAppMonitorConfig)serviceProvider.GetService(typeof(IAppMonitorConfig));

            // Create a new instance of the API middleware, can't resolve services yet by this point
            // from the serviceprovider so have to take the previously construted coordinator and pass it
            // in instead
            APIMiddleware middleware = new APIMiddleware(
                (IAppMonitorCoordinator)serviceProvider.GetService(typeof(IAppMonitorCoordinator)),
                configuration);

            // Set up the given hub endpoints based on the configuration when the first negotiation happens
            // reason this is inside here rather than outside is that the ASP.Net middleware and SignalR's own 
            // internal overriding of the middleware have different lifetimes and delegation
            app.Map($"{configuration.SignalREndpoint}/negotiate", map => MapHubs(app, configuration));

            // Work that doesn't affect the response processing
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.Contains(configuration.ApiEndpoint))
                {
                }
                await next.Invoke();
#warning TODO: Logging here! As it doesn't affect the response
            });

            app.Run(async context =>
            {
                if (context.Request.Path.Value.Contains(configuration.ApiEndpoint))
                {
                    await context.Response.WriteAsync("You hit the api.");
                }
            });

            return app;
        }

        /// <summary>
        /// Set up the SignalR Hubs (Be careful about interferance from Middleware overloading
        /// the SignalR internally defined middleware)
        /// </summary>
        /// <param name="app"></param>
        private static void MapHubs(IApplicationBuilder app, IAppMonitorConfig configuration)
        {
            app.UseEndpoints(endpoints =>
            {
                // Ensure the signalR hub is mapped
                endpoints.MapHub<AppMonitorHub>(configuration.SignalREndpoint, options =>
                {
                    //options.Transports = HttpTransportType.LongPolling;
                });
            });
        }
    }
}
