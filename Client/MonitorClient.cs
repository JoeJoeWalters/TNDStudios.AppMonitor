using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TNDStudios.AppMonitor.Client
{
    public class MonitorClient
    {
        private HubConnection connection;
        private String applicationName;

        public MonitorClient(String applicationName, String uri)
        {
            this.applicationName = applicationName;
            connection = new HubConnectionBuilder()
                .WithUrl(new Uri(uri))
                .WithAutomaticReconnect(new MonitorClientRetryPolicy())
                .Build();
        }

        public Boolean Connect()
        {
            Int32 attempts = 5;
            CancellationToken token = new CancellationToken();

            // Keep trying to until we can start or the token is canceled.
            while (true)
            {
                try
                {
                    connection.StartAsync(token).Wait();
                    return true;
                }
                catch when (token.IsCancellationRequested)
                {
                    return false;
                }
                catch
                {
                    // Failed to connect, trying again in 5000 ms.
                    attempts--;
                    if (0 == attempts) return false;
                    Task.Delay(5000);
                }
            }
        }

        public Boolean Heartbeat()
        {
            if (connection.State == HubConnectionState.Connected)
            {
                try
                {
                    Int32 randomSeconds = (Int32)(2.0 * 60.0 * (new Random().NextDouble()));
                    DateTime nextRunTime = DateTime.UtcNow.AddSeconds(randomSeconds);
                    connection.InvokeAsync("SendHeartbeat", applicationName, nextRunTime).Wait();
                    Console.WriteLine($"Sent Heartbeat - Next Expected Run is {nextRunTime.ToString()}");
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

        public Boolean Error(String error)
        {
            if (connection.State == HubConnectionState.Connected)
            {
                try
                {
                    connection.InvokeAsync("SendError", applicationName, error).Wait();
                    Console.WriteLine("Sent Error");
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

        public Boolean Send(String property, Int32 metric)
        {
            if (connection.State == HubConnectionState.Connected)
            {
                try
                {
                    connection.InvokeAsync("SendMetric", applicationName, property, metric.ToString()).Wait();
                    Console.WriteLine("Sent Metric");
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
