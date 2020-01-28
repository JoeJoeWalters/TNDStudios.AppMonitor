using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TNDStudios.AppMonitor.Core
{    
    public class AppMonitorControllerBase : Controller
    {
        /// <summary>
        /// INjected core service from the Singleton provided at setup
        /// </summary>
        private readonly IAppMonitorCoordinator coordinator;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="appMonitorCore"></param>
        public AppMonitorControllerBase(IAppMonitorCoordinator coordinator) 
        {
            this.coordinator = coordinator;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
