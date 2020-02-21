using System;
using System.Collections.Generic;
using System.Threading;
using TNDStudios.AppMonitor.Client;

namespace Transmitter
{
    class Program
    {
        private const String monitorServiceUri = "https://localhost:44392/api/appmonitor";

        private static List<AppMonitorConnector> applications = new List<AppMonitorConnector>();
        private static AppMonitorConnector RandomApplication()
            => applications[(Int32)((new Random()).NextDouble() * (Double)applications.Count)];

        static Program()
        {
            applications.Add(new AppMonitorConnector("Mail Application", monitorServiceUri));
            applications.Add(new AppMonitorConnector("FTP Application", monitorServiceUri));
            applications.Add(new AppMonitorConnector("Invoicing Application", monitorServiceUri));
            applications.Add(new AppMonitorConnector("Payroll Application", monitorServiceUri));
            applications.Add(new AppMonitorConnector("Client 1 Application", monitorServiceUri));
            applications.Add(new AppMonitorConnector("Client 2 Application", monitorServiceUri));
            applications.Add(new AppMonitorConnector("Client 3 Application", monitorServiceUri));
        }

        static void Main(string[] args)
        {
            while (true)
            {
                AppMonitorConnector monitorClient = RandomApplication();

                Double randomVal = (new Random()).NextDouble();

                if (randomVal > 0.9)
                    monitorClient.SendError("Shit went sideways!");

                monitorClient.SendMetric("Record", randomVal * 100D).Wait();
                Thread.Sleep((Int32)((new Random()).NextDouble() * 2000));
            }
        }
    }
}
