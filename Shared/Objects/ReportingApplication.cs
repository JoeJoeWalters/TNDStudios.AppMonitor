using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TNDStudios.AppMonitor.Objects
{
    [JsonObject]
    public class ReportingApplication : ReportingObjectBase
    {
        /// <summary>
        /// Name of the application
        /// </summary>
        [JsonProperty(PropertyName = "name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public String Name { get; set; }

        /// <summary>
        /// Summary of metrics per path (not individually received metrics)
        /// </summary>
        [JsonProperty(PropertyName = "metrics", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Dictionary<String, ReportingMetricGroup> Metrics { get; set; }

        [JsonProperty(PropertyName = "errors", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<ReportingError> Errors { get; set; }

        [JsonProperty(PropertyName = "nextRunTime", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime NextRunTime { get; set; }

        /// <summary>
        /// Default constructor using base to set up core values
        /// </summary>
        public ReportingApplication() : base()
        {
            Name = String.Empty;
            Metrics = new Dictionary<String, ReportingMetricGroup>();
            Errors = new List<ReportingError>();
            NextRunTime = DateTime.MinValue;
        }
    }
}
