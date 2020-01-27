using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TNDStudios.AppMonitor.Core
{
#warning TODO: investigate area routing so don't have to define base class inheritance by method
    // https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/areas?view=aspnetcore-2.1

    
    [Route("api/appmonitor")]
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
