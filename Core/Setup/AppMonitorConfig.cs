using System;

namespace TNDStudios.AppMonitor.Core
{
    public interface IAppMonitorConfig
    {
        /// <summary>
        /// The endpoint to set up where SignalR coordination will happen
        /// </summary>
        String SignalREndpoint { get; set; }

        /// <summary>
        /// The endpoint to set up where the application when get all other API based actions
        /// </summary>
        String ApiEndpoint { get; set; }

        /// <summary>
        /// THe interval for the background task between runs to tidy up statistics etc.
        /// </summary>
        Double MetricMonitorInterval { get; set; }
    }

    public class AppMonitorConfig : IAppMonitorConfig
    {
        /// <summary>
        /// The endpoint to set up where SignalR coordination will happen
        /// </summary>
        public String SignalREndpoint { get; set; }

        /// <summary>
        /// The endpoint to set up where the application when get all other API based actions
        /// </summary>
        public String ApiEndpoint { get; set; }

        /// <summary>
        /// THe interval for the background task between runs to tidy up statistics etc.
        /// </summary>
        public Double MetricMonitorInterval { get; set; } = 60; // Default to every minute
    }
}
