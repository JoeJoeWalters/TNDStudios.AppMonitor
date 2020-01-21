using System;
using System.Collections.Generic;
using System.Threading;
using TNDStudios.AppMonitor.Client;

namespace Transmitter
{
    class Program
    {
        private const String monitorServiceUri = "https://localhost:44392/signalr/appmonitor";

        private static List<MonitorClient> applications = new List<MonitorClient>();
        private static MonitorClient RandomApplication()
            => applications[(Int32)((new Random()).NextDouble() * (Double)applications.Count)];

        static Program()
        {
            applications.Add(new MonitorClient("Mail Application", monitorServiceUri));
            applications.Add(new MonitorClient("FTP Application", monitorServiceUri));
            applications.Add(new MonitorClient("Invoicing Application", monitorServiceUri));
            applications.Add(new MonitorClient("Payroll Application", monitorServiceUri));
            applications.Add(new MonitorClient("Client 1 Application", monitorServiceUri));
            applications.Add(new MonitorClient("Client 2 Application", monitorServiceUri));
            applications.Add(new MonitorClient("Client 3 Application", monitorServiceUri));
            applications.ForEach(app => app.Connect());
        }

        static void Main(string[] args)
        {
            while (true)
            {
                MonitorClient monitorClient = RandomApplication();

                Double randomVal = (new Random()).NextDouble();

                if (randomVal < 0.1)
                    monitorClient.SendHeartbeat();

                if (randomVal > 0.9)
                    monitorClient.SendError("Shit went sideways!");

                monitorClient.SendMetric("Record", (Int32)(randomVal * 100));
                Thread.Sleep((Int32)((new Random()).NextDouble() * 2000));
            }
        }
    }
}
