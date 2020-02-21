using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TNDStudios.AppMonitor.Service
{
    /// <summary>
    /// Client for .Net Core but written via Standard Library to handle trasmission of metrics,
    /// heartbeats, errors etc. to the central hub
    /// </summary>
    public class MonitorClient
    {
        /// <summary>
        /// The hub connection once it has been established
        /// </summary>
        private HubConnection connection;

        /// <summary>
        /// The name of the application that this client is representing
        /// </summary>
        private String applicationName;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="applicationName">The name of the host application</param>
        /// <param name="uri">The uri of the hub to transmit to</param>
        public MonitorClient(String applicationName, String serviceUri)
        {
            this.applicationName = applicationName;
            connection = new HubConnectionBuilder()
                .WithUrl(new Uri(serviceUri))
                .WithAutomaticReconnect(new MonitorClientRetryPolicy())
                .Build();
        }

        /// <summary>
        /// Connect to the remote service to start communication
        /// </summary>
        /// <returns>Status of the connection as a Boolean</returns>
        public Boolean Connect()
        {
            Int32 attempts = 5; // Limit the retry attempts
            CancellationToken token = new CancellationToken(); // Provide a fresh cancellation token to use

            // Keep trying to until we can start or the token is canceled.
            while (true)
            {
                try
                {
                    // Attempt the connection, associated errors from this will be trapped individually
                    connection.StartAsync(token).Wait();
                    return true;
                }
                catch when (token.IsCancellationRequested)
                {
                    // Total cancellation from the service we are connecting to
                    return false;
                }
                catch
                {
                    // Failed to connect, but not cancelled, try again in 5000 ms.
                    attempts--;
                    if (0 == attempts) return false;
                    Task.Delay(5000);
                }
            }
        }
        
        /// <summary>
        /// Transmit an error from the application to the hub
        /// </summary>
        /// <param name="error">The string representation of the error</param>
        /// <returns>If the error was sent to the hub</returns>
        public Boolean SendError(String error)
        {
            // Is the hub connected to the client?
            if (connection.State == HubConnectionState.Connected)
            {
                try
                {
                    connection.InvokeAsync("SendError", applicationName, error).Wait();
                }
                catch
                {
                    return false;
                }
            }
            else
                return false;

            return true;
        }

        /// <summary>
        /// Send a metric to the hub for this application
        /// </summary>
        /// <param name="property">The "name" of the metric property</param>
        /// <param name="metric">The value of the metric</param>
        /// <returns></returns>
        public Boolean SendMetric(String property, Double metric)
        {
            // Is the hub connected to the client?
            if (connection.State == HubConnectionState.Connected)
            {
                try
                {
                    connection.InvokeAsync("SendMetric", applicationName, property, metric).Wait();
                }
                catch
                {
                    return false;
                }
            }
            else
                return false;

            return true;
        }
    }
}
