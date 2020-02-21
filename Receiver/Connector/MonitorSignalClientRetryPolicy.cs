using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace TNDStudios.AppMonitor.Service
{

    public class MonitorClientRetryPolicy : IRetryPolicy
    {
        private readonly Random random = new Random();

        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            // If we've been reconnecting for less than 60 seconds so far,
            // wait between 0 and 10 seconds before the next reconnect attempt.
            if (retryContext.ElapsedTime < TimeSpan.FromSeconds(60))
            {
                return TimeSpan.FromSeconds(random.Next() * 10);
            }
            else
            {
                // If we've been reconnecting for more than 60 seconds so far, stop reconnecting.
                return null;
            }
        }
    }
}
