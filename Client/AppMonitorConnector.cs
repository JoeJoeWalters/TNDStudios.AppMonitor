using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TNDStudios.AppMonitor.Client
{
    public class AppMonitorConnector
    {
        /// <summary>
        /// Base endpoint for API calls to send Errors and metrics to
        /// </summary>
        private readonly String applicationName = String.Empty;
        private readonly String endpoint = String.Empty;
        private String errorEndpoint { get => $"{endpoint}/errors"; }
        private String metricEndpoint { get => $"{endpoint}/metrics"; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="endpoint">The registered endpoint to communicate with</param>
        public AppMonitorConnector(String applicationName, String endpoint)
        {
            this.applicationName = applicationName;
            this.endpoint = endpoint;
        }

        public async Task<Boolean> SendError(String error)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(errorEndpoint);
                HttpResponseMessage response = client.SendAsync(new HttpRequestMessage()).Result;
            }
            return true;
        }

        public async Task<Boolean> SendMetric(String property, Double metric)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(metricEndpoint);
                HttpResponseMessage response = client.SendAsync(new HttpRequestMessage()).Result;
            }
            return true;
        }
    }
}
