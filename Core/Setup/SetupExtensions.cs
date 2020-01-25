using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace TNDStudios.AppMonitor.Core
{
    public static class SetupExtensions
    {
        /// <summary>
        /// Extension of the Service Collection so it can be added in Web Application startup
        /// </summary>
        /// <param name="serviceCollection">The Sercice Collection injected into the Web Application</param>
        /// <returns>The modified Service Collection</returns>
        public static IServiceCollection AddAppMonitor(this IServiceCollection services)
        {
            // Add a singleton for the App Monitor Core so it can be injected in to constructors etc.
            services.AddSingleton<IAppMonitorCoordinator>(new AppMonitorCoordinator() { });

            // Make sure SignalR is added as a service
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });

            services.AddHostedService<MetricMonitor>();

            return services;
        }

        /// <summary>
        /// Extension of the application builder so that the Web Application startup
        /// </summary>
        /// <param name="applicationBuilder">The Application Builder injected into the Web Application</param>
        /// <returns>The modified Application Builder</returns>
        public static IApplicationBuilder UseAppMonitor(this IApplicationBuilder app,
            AppMonitorConfig configuration)
        {
            // Enforce Routing Usage
            app.UseRouting();

            // Set up the given endpoints based on the configuration
            app.UseEndpoints(endpoints =>
                endpoints.MapHub<AppMonitorHubBase>(configuration.SignalREndpoint, options =>
                {
                    //options.Transports = HttpTransportType.LongPolling;
                }));


            return app;
        }
    }
}
