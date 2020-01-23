using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.AppMonitor.Objects
{
    public class ReportingInterval
    {
        /// <summary>
        /// The time span that this interval accounts for
        /// </summary>
        public TimeSpan Span { get; set; }

        /// <summary>
        /// The start of the interval period with the span indicating the length
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// The Metric (whether that is error count or actual metrics)
        /// </summary>
        public Double Value { get; set; }
    }
}
