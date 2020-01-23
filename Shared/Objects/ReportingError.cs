using Newtonsoft.Json;
using System;

namespace TNDStudios.AppMonitor.Objects
{
    [JsonObject]
    public class ReportingError : ReportingObjectBase
    {
        /// <summary>
        /// The time that the error was received (defaulting to now when not passed in)
        /// </summary>
        [JsonProperty(PropertyName = "time", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime Time { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The error that was received
        /// </summary>
        [JsonProperty(PropertyName = "message", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public String Message { get; set; }

        /// <summary>
        /// Default constructor using base to set up core values
        /// </summary>
        public ReportingError() : base() { }
    }
}
