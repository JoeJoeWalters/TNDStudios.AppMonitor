using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TNDStudios.AppMonitor.Objects
{
    [JsonObject]
    public class ReportingApplication : ReportingObjectBase
    {
        /// <summary>
        /// Locking objects so we can handle async calls with wiping out existing data
        /// </summary>
        private Object metricLock = new Object(); // Locking for adding new metric paths

        /// <summary>
        /// Name of the application
        /// </summary>
        [JsonProperty(PropertyName = "name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public String Name { get; set; }

        /// <summary>
        /// Summary of metrics per path (not individually received metrics)
        /// </summary>
        [JsonProperty(PropertyName = "metrics", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Dictionary<String, ReportingMetrics> Metrics { get; set; }

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
            Metrics = new Dictionary<String, ReportingMetrics>() { { "Errors", new ReportingMetrics() } };
            Errors = new List<ReportingError>();
            NextRunTime = DateTime.MinValue;
        }

        /// <summary>
        /// Add data to metrics, does any locking needed to add a new metric group if one doesn't
        /// already exist then passes the data down in to the reporting group to handle locking when
        /// adding new summaries
        /// </summary>
        /// <param name="path">The metric path for the reporting group</param>
        /// <param name="metric">The value of the current metric</param>
        /// <returns></returns>
        public Boolean AddMetric(String path, Double metric)
        {
            // Does this metric path not exist?
            if (!Metrics.ContainsKey(path))
            {
                // Lock the metrics whilst we add the path so we don't have another 
                // call interfering
                lock (metricLock)
                {
                    // Check that another call hasn't added the key whilst we were locking
                    if (!Metrics.ContainsKey(path))
                        Metrics[path] = new ReportingMetrics(); // Add the new metric group
                }
            }

            // Pass in the metric to be handled by the metric group (there might be other locking)
            return Metrics[path].AddMetric(metric);
        }

        /// <summary>
        /// Go through each path and prune the metrics
        /// </summary>
        public void IndexMetrics(DateTime now)
        {
            // No need to lock as we are only going to kill items that 
            // are unlikely to be written to (e.g. old items out of the current
            // timeframe)
            foreach (String key in Metrics.Keys)
                Metrics[key].IndexMetrics(now);
        }
    }
}
