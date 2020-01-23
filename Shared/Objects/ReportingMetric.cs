using Newtonsoft.Json;
using System;

namespace TNDStudios.AppMonitor.Objects
{
    [JsonObject]
    public class ReportingMetric : ReportingObjectBase
    {
        [JsonProperty(PropertyName = "path", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public String Path { get; set; }

        [JsonProperty(PropertyName = "value", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Double Value { get; set; }

        /// <summary>
        /// Default constructor using base to set up core values
        /// </summary>
        public ReportingMetric() : base() { }
    }
}
