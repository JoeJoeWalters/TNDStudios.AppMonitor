using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.AppMonitor.Objects
{
    public class ReportingMetricGroup
    {
        /// <summary>
        /// Sum of metrics for each minute in the last 60 minutes
        /// </summary>
        public Dictionary<DateTime, Double> Minutes { get; set; }

        /// <summary>
        /// Sum of metrics for each hour in the last calendar day
        /// </summary>
        public Dictionary<DateTime, Double> Hours { get; set; }

        /// <summary>
        /// Sum of metrics for each day
        /// </summary>
        public Dictionary<DateTime, Double> Days { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ReportingMetricGroup()
        {
            Minutes = new Dictionary<DateTime, Double>();
            Hours = new Dictionary<DateTime, Double>();
            Days = new Dictionary<DateTime, Double>();
        }
    }
}
