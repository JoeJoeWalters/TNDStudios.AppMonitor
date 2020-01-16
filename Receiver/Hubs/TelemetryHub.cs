using TNDStudios.AppMonitor.Core;

namespace TNDStudios.AppMonitor.Service
{
    public class TelemetryHub : TelemetryHubBase
    {
        public TelemetryHub(IAppMonitorCore appMonitorCore) : base(appMonitorCore) { }
    }
}
