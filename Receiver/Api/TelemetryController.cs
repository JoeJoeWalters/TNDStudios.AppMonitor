using TNDStudios.AppMonitor.Core;

namespace TNDStudios.AppMonitor.Service
{
    public class TelemetryController : TelemetryControllerBase
    {
        public TelemetryController(IAppMonitorCore appMonitorCore) : base(appMonitorCore) { }
    }
}