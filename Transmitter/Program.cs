using System;
using System.Collections.Generic;
using System.Threading;
using TNDStudios.AppMonitor.Client;

namespace Transmitter
{
    class Program
    {
        private static List<PlugIn> applications = new List<PlugIn>();
        private static PlugIn RandomApplication()
            =>applications[(Int32)((new Random()).NextDouble() * (Double)applications.Count)];

        static Program()
        {
            applications.Add(new PlugIn("Mail Application", "https://localhost:44392/signalr/telemetry"));
            applications.Add(new PlugIn("FTP Application", "https://localhost:44392/signalr/telemetry"));
            applications.Add(new PlugIn("Invoicing Application", "https://localhost:44392/signalr/telemetry"));
            applications.Add(new PlugIn("Payroll Application", "https://localhost:44392/signalr/telemetry"));
            applications.Add(new PlugIn("Client 1 Application", "https://localhost:44392/signalr/telemetry"));
            applications.Add(new PlugIn("Client 2 Application", "https://localhost:44392/signalr/telemetry"));
            applications.Add(new PlugIn("Client 3 Application", "https://localhost:44392/signalr/telemetry"));
            applications.ForEach(app => app.Connect());
        }

        static void Main(string[] args)
        {
            while (true)
            {
                PlugIn telemetryHandler = RandomApplication();

                Double randomVal = (new Random()).NextDouble();

                if (randomVal < 0.1)
                    telemetryHandler.Heartbeat();

                if (randomVal > 0.9)
                    telemetryHandler.Error("Shit went sideways!");

                telemetryHandler.Send("Record", (Int32)(randomVal * 100));
                Thread.Sleep((Int32)((new Random()).NextDouble() * 2000));
            }
        }
    }
}
