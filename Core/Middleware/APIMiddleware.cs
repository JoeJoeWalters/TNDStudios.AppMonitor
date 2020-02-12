using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNDStudios.AppMonitor.Core
{
    public class APIMiddleware
    {
        /// <summary>
        /// Injected items
        /// </summary>
        private readonly IAppMonitorCoordinator coordinator;
        private readonly IAppMonitorConfig config;

        /// <summary>
        /// Default constructor to allow dependency injection of coordinator and logging
        /// </summary>
        /// <param name="coordinator">Singleton for the coordinator</param>
        /// <param name="config">Singleton for the configuration that was defined in startup</param>
        public APIMiddleware(IAppMonitorCoordinator coordinator, IAppMonitorConfig config) 
        {
            this.coordinator = coordinator;
            this.config = config;
        }
    }
}
