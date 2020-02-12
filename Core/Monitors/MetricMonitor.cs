﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TNDStudios.AppMonitor.Core
{
    public class MetricMonitor : IHostedService, IDisposable
    {
        /// <summary>
        /// Injected items
        /// </summary>
        private readonly ILogger log;
        private readonly IAppMonitorCoordinator coordinator;

        /// <summary>
        /// Timer used for scheduling of the monitor
        /// </summary>
        private Timer timer;

        /// <summary>
        /// Default constructor with injected items
        /// </summary>
        /// <param name="log">The logger for the app</param>
        /// <param name="coordinator">Singleton for the coordinator</param>
        public MetricMonitor(ILogger<MetricMonitor> log, IAppMonitorCoordinator coordinator)
        {
            this.log = log;
            this.coordinator = coordinator;
        }

        /// <summary>
        /// Start up the monitor service
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            log.LogInformation("Starting 'Metric Monitor' scheduler");
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
            return Task.CompletedTask;
        }

        /// <summary>
        /// Do the actual work for the monitor
        /// </summary>
        /// <param name="state"></param>
        private void DoWork(object state)
        {
            log.LogInformation("Indexing Metrics");
            coordinator.IndexMetrics(); // Index and clean the metrics in memory etc.
        }

        /// <summary>
        /// Stop the monitor (stop the scheduling but don't dispose yet as it could restart)
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            log.LogInformation("Stopping 'Metric Monitor' scheduler");
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Destroy the scheduler completely
        /// </summary>
        public void Dispose()
        {
            timer?.Dispose(); // Clean up the timer and scheduling
        }
    }
}