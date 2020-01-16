using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TNDStudios.AppMonitor.Objects;

namespace TNDStudios.AppMonitor.Core
{
    public class TelemetryHubBase : Hub
    {
        /// <summary>
        /// INjected core service from the Singleton provided at setup
        /// </summary>
        private readonly IAppMonitorCore appMonitorCore;

        /// <summary>
        /// On absolute start then set up the arrays needed across sessions
        /// </summary>
        static TelemetryHubBase()
        {
        }

        /// <summary>
        /// Default constructor with the App Monitor Core injected into it
        /// </summary>
        public TelemetryHubBase(IAppMonitorCore appMonitorCore)
        {
            this.appMonitorCore = appMonitorCore;
        }

        /// <summary>
        /// Generate a reporting summary for warboards etc.
        /// </summary>
        /// <returns>The consolidated reporting summary</returns>
        public ReportingSummary GetReportingSummary()
        {
            // Create a blank report as we won't be returning everything
            ReportingSummary reportingSummary =
                new ReportingSummary()
                {
                    Applications = new List<ReportingApplication>()
                };

            // Loop all the applications

            return reportingSummary;
        }

        public async Task SendMetric(string applicationName, string property, string metric)
        {
            ReportingApplication application = appMonitorCore.GetApplication(applicationName);
            application.Metrics.Add(new ReportingMetric() { Property = property, Value = metric });
            await Clients.All.SendAsync("ReceiveMetric", applicationName, property, metric);
        }

        public async Task SendHeartbeat(string applicationName, DateTime nextRunTime)
        {
            ReportingApplication application = appMonitorCore.GetApplication(applicationName);
            application.NextRunTime = nextRunTime;
            application.Heartbeats.Add(new ReportingHeartbeat() { NextRunTime = nextRunTime });
            await Clients.All.SendAsync("ReceiveHeartbeat", applicationName, nextRunTime);
        }

        public async Task SendError(string applicationName, string errorMessage)
        {
            ReportingApplication application = appMonitorCore.GetApplication(applicationName);
            application.Errors.Add(new ReportingError() { Message = errorMessage });
            await Clients.All.SendAsync("ReceiveError", applicationName, errorMessage);
        }
    }
}
