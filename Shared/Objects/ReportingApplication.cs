using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TNDStudios.AppMonitor.Objects
{
    [JsonObject]
    public class ReportingApplication : ReportingObjectBase
    {
        [JsonProperty(PropertyName = "name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public String Name { get; set; }

        [JsonProperty(PropertyName = "metrics", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<ReportingMetric> Metrics { get; set; }

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
            Metrics = new List<ReportingMetric>();
            Errors = new List<ReportingError>();
            NextRunTime = DateTime.MinValue;
        }
    }
}
