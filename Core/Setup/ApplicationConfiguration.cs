using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.AppMonitor.Core
{
    public class ApplicationConfiguration
    {
        /// <summary>
        /// The endpoint to set up where SignalR coordination will happen
        /// </summary>
        public String SignalREndpoint { get; set; }

        /// <summary>
        /// The endpoint to set up where the application when get all other API based actions
        /// </summary>
        public String ApiEndpoint { get; set; }
    }
}
