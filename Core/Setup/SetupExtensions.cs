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
            services.AddSingleton<IAppMonitorCore>(new AppMonitorCore() { });

            return services;
        }

        /// <summary>
        /// Extension of the application builder so that the Web Application startup
        /// </summary>
        /// <param name="applicationBuilder">The Application Builder injected into the Web Application</param>
        /// <returns>The modified Application Builder</returns>
        public static IApplicationBuilder UseAppMonitor(this IApplicationBuilder applicationBuilder)
            => applicationBuilder;
    }
}
