using TNDStudios.AppMonitor.Core;

namespace TNDStudios.AppMonitor.Service
{
    public class TelemetryHub : TelemetryHubBases
    {
        public TelemetryHub(IAppMonitorCore appMonitorCore) : base(appMonitorCore) { }
    }
}
