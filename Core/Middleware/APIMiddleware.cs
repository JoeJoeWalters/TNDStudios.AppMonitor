﻿using Microsoft.AspNetCore.Http;
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
        private const String metricEndpoint = "/metric";
        private const String errorEndpoint = "/error";

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
                //String body = context.Request.Body;

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
                    // Metric sent to the API (Rather than via SignalR transmission)
                    // ReportingError error = 
                }
                else if (path.EndsWith(errorEndpoint))
                {
                    // Error sent to the API (Rather than via SignalR transmission)
                }
                else
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            return Task.FromResult((successResponses.Contains((HttpStatusCode)context.Response.StatusCode)));
        }
    }
}
