using Newtonsoft.Json;
using System;

namespace TNDStudios.AppMonitor.Objects
{
    [JsonObject]
    public class ReportingError : ReportingObjectBase
    {
        [JsonProperty(PropertyName = "message", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public String Message { get; set; }

        /// <summary>
        /// Default constructor using base to set up core values
        /// </summary>
        public ReportingError() : base() { }
    }
}
