using TNDStudios.AppMonitor.Core;

namespace TNDStudios.AppMonitor.Service
{
    public class AppMonitorHub : AppMonitorHubBase
    {
        public AppMonitorHub(IAppMonitorCoordinator coordinator) : base(coordinator) { }
    }
}
