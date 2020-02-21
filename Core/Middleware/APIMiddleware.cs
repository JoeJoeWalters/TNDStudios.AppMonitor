using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TNDStudios.AppMonitor.Objects;

namespace TNDStudios.AppMonitor.Core
{
    public class APIMiddleware
    {
        private const String summaryEndpoint = "/summary";
        private const String metricEndpoint = "/metrics";
        private const String errorEndpoint = "/errors";

        /// <summary>
        /// Injected items
        /// </summary>
        private readonly IAppMonitorCoordinator coordinator;
        private readonly IAppMonitorConfig config;

        /// <summary>
        /// Default constructor to allow dependency injection of coordinator and logging
        /// </summary>
        /// <param name="coordinator">Singleton for the coordinator</param>
        /// <param name="config">Singleton for the configuration that was defined in startup</param>
        public APIMiddleware(IAppMonitorCoordinator coordinator, IAppMonitorConfig config) 
        {
            this.coordinator = coordinator;
            this.config = config;
        }

        public Task<Boolean> ProcessRequest(HttpContext context)
        {
            // Responses which are considered a successful outcome
            List<HttpStatusCode> successResponses =
                new List<HttpStatusCode>() 
                { 
                    HttpStatusCode.OK 
                };

            String path = context.Request.Path.Value.ToLower();
            if (path.Contains(config.ApiEndpoint.ToLower()))
            {
                try
                {
                    String body = String.Empty;
                    using (StreamReader reader = new StreamReader(context.Request.Body))
                        body = reader.ReadToEnd();

                    // Decipher the endpoint 
                    if (path.EndsWith(summaryEndpoint))
                    {
                        // Get the summary to populate the UI etc.
                        using (StreamWriter writer = new StreamWriter(context.Response.Body))
                        {
                            writer.Write(JsonConvert.SerializeObject(coordinator.Summary()));
                        }
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                    }
                    else if (path.EndsWith(metricEndpoint))
                    {
#warning TODO: This is not the right error object type, this is the storage, new objects needed specificially for the API Endpoints
                        ReportingError error = JsonConvert.DeserializeObject<ReportingError>(body);
                        if (error != null)
                            throw new Exception("Error payload malformed");
                        else
                        {
                            // Metric sent to the API (Rather than via SignalR transmission)
                            coordinator.RegisteredHub.SendMetric("", "", 1).Wait();
                        }
                    }
                    else if (path.EndsWith(errorEndpoint))
                    {
                        // Error sent to the API (Rather than via SignalR transmission)
                        // Forward the error on to the hub to record the error and handle transmission
                        // The hub will have been registered with the coordinator when it was instantiated
                        coordinator.RegisteredHub.SendError("", "").Wait();
                    }
                    else
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    (new MemoryStream(Encoding.UTF8.GetBytes(ex.Message))).CopyTo(context.Response.Body);
                }
            }

            return Task.FromResult((successResponses.Contains((HttpStatusCode)context.Response.StatusCode)));
        }
    }
}
