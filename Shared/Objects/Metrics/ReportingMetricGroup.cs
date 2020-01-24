using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.AppMonitor.Objects
{
    public class ReportingMetricGroup
    {
        /// <summary>
        /// Locking object used for when adding data across the various pots of data
        /// </summary>
        private Object lockingObject = new Object();

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

        /// <summary>
        /// Add the metric to the relevant groups and handle any locking
        /// </summary>
        /// <param name="metric">The value of the metric</param>
        /// <returns></returns>
        public Boolean AddMetric(Double metric)
        {
            // Calculate the intervals we should be writing to
            DateTime now = DateTime.UtcNow;
            DateTime minuteInterval = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            DateTime hourInterval = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
            DateTime dayInterval = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

            // Create a transaction lock (also used for periodically sorting out the
            // data elsewhere so is a common lock)
            lock(lockingObject)
            {
                AddMetricToPot(Minutes, minuteInterval, metric); // Add to the minutes summary
                AddMetricToPot(Hours, hourInterval, metric); // Add to the hours summary
                AddMetricToPot(Days, dayInterval, metric); // Add to the days summary
            }

            return true;
        }

        /// <summary>
        /// Add data to a given metric pot
        /// </summary>
        /// <param name="pot">The pot (days, hours and minutes)</param>
        /// <param name="key">The interval key (start of the interval)</param>
        /// <param name="metric">The metric to add to the summary</param>
        private void AddMetricToPot(Dictionary<DateTime, Double> pot, DateTime key, Double metric)
        {
            if (!pot.ContainsKey(key))
                pot.Add(key, metric);
            else
                pot[key] += metric;
        }

        /// <summary>
        /// Reindexes the metrics and whilst it is happening it will lock new metrics being added
        /// </summary>
        /// <returns>Success or failure</returns>
        public Boolean IndexMetrics()
        {
            lock(lockingObject)
            {

            }

            return true;
        }
    }
}
