using System;
using System.IO;

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
        /// Where the background task will serialise data to and on startup where it will be loaded from
        /// </summary>
        String SaveLocation { get; set; }

        /// <summary>
        /// THe interval for the background task between runs to tidy up statistics etc.
        /// </summary>
        Double BackgroundTaskInterval { get; set; }

        /// <summary>
        /// Go over the values in the configuration and validate them and also fix any issues
        /// such as leading characters etc,
        /// </summary>
        /// <returns></returns>
        Boolean Parse();
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
        /// Where the background task will serialise data to and on startup where it will be loaded from
        /// </summary>
        public String SaveLocation { get; set; }

        /// <summary>
        /// THe interval for the background task between runs to tidy up statistics etc.
        /// </summary>
        public Double BackgroundTaskInterval { get; set; } = 60; // Default to every minute

        /// <summary>
        /// Go over the values in the configuration and validate them and also fix any issues
        /// such as leading characters etc,
        /// </summary>
        /// <returns></returns>
        public Boolean Parse()
        {
            if ((SignalREndpoint ?? String.Empty).Trim() == String.Empty)
                    throw new Exception("No SignalR Endpoint defined for Application Monitor");
            else
            {
                // Clean endpoint string
                if (!SignalREndpoint.StartsWith('/'))
                    SignalREndpoint = $"/{SignalREndpoint}".Trim();
            }

            if ((ApiEndpoint ?? String.Empty).Trim() == String.Empty)
                    throw new Exception("No API Endpoint defined for Application Monitor");
            {
                // Clean endpoint string
                if (!ApiEndpoint.StartsWith('/'))
                    ApiEndpoint = $"/{ApiEndpoint}".Trim();
            }

            if ((SaveLocation ?? String.Empty).Trim() == String.Empty)
                throw new Exception("No save location defined for Application Monitor");

            if (BackgroundTaskInterval < 60)
                throw new Exception("Application Monitor Background Task Interval is too short at less than 60 seconds");

            return true;
        }
    }
}
