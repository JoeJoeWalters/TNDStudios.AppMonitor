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

        /// <summary>
        /// Default constructor to allow dependency injection of coordinator and logging
        /// </summary>
        /// <param name="coordinator">Singleton for the coordinator</param>
        public APIMiddleware(IAppMonitorCoordinator coordinator) 
        {
            this.coordinator = coordinator;
        }
    }
}
