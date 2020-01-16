using Microsoft.AspNetCore.Mvc;
using TNDStudios.AppMonitor.Core;

namespace TNDStudios.AppMonitor.Service
{
    [Route("api/[controller]")]
    public class AppMonitorController : AppMonitorControllerBase
    {
        public AppMonitorController(IAppMonitorCoordinator coordinator) : base(coordinator) { }
    }
}