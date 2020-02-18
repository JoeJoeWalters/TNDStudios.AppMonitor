using System;
using System.Collections.Generic;
using System.Linq;
using TNDStudios.AppMonitor.Objects;

namespace TNDStudios.AppMonitor.Core
{
    public interface IAppMonitorCoordinator
    {       
        /// <summary>
        /// Get a summary of the state of the system
        /// </summary>
        /// <returns></returns>
        ReportingSummary Summary();

        /// <summary>
        /// Return or create and return a new application with a given key
        /// </summary>
        /// <param name="applicationName">The key for the application</param>
        /// <returns>The Application Object</returns>
        ReportingApplication GetApplication(String applicationName);

        /// <summary>
        /// Go through each application and prune the metrics
        /// </summary>
        void IndexMetrics();

        /// <summary>
        /// Load the metrics from the cache location (if the service restarted etc.)
        /// </summary>
        /// <param name="loadDirectory"></param>
        /// <returns></returns>
        Boolean LoadData(String loadDirectory);

        /// <summary>
        /// Save the current in-memory metrics to the cache location (so if the service reboots it can be reloaded)
        /// </summary>
        /// <param name="saveDirectory"></param>
        /// <returns></returns>
        Boolean SaveData(String saveDirectory);
    }

    public class AppMonitorCoordinator : IAppMonitorCoordinator
    {
        // Locking object for the dictionary rather than putting a lock on the dictionary itself
        // so we can dirty read the metrics without slowing down the app but creates and deletes
        // can be locked, we also don't want to lock array pushes to sub-items
        private Object lockingObject = new Object();

        /// <summary>
        /// Dictionary of applications that have been reported against by the clients
        /// </summary>
        private Dictionary<String, ReportingApplication> applications { get; set; }

        /// <summary>
        /// Get a summary of the core service to return to the client so they can get 
        /// a summary of what is happening in the application map
        /// </summary>
        /// <returns></returns>
        public ReportingSummary Summary()
        {
#warning TODO: Very rough summary based on actual data
            return new ReportingSummary() { Applications = this.applications.Values.ToList() };
        }

        /// <summary>
        /// Check to see if the application has been received before, if not create it
        /// and return the value that was either found or created
        /// </summary>
        /// <param name="applicationName"></param>
        /// <returns>The reporting application object</returns>
        public ReportingApplication GetApplication(String applicationName)
        {
            // Format the application name so it's consistent
            String searchString = (applicationName ?? String.Empty).ToLower().Trim();

            // See if the application has already been received
            if (!applications.ContainsKey(searchString))
            {
                // Lock the applications object whilst we are inserting
                // as it could be getting another request at the same time
                // and we don't want to overwrite it
                lock (lockingObject)
                {
                    // Now the application is locked, check again just incase someone 
                    // else wrote whilst we were waiting to lock
                    if (applications.ContainsKey(searchString))
                        return applications[searchString];

                    // After the second check the application still wasn't there
                    // Create the new application for reporting and adding to the cache
                    ReportingApplication newApplication = new ReportingApplication() { Name = applicationName.Trim() };

                    // Cache it for someone else
                    applications[searchString] = newApplication;

                    // Return the application
                    return newApplication;
                } // Unlock so others can create applications
            }
            else
                return applications[searchString]; // Send the existing one back
        }

        /// <summary>
        /// Go through each application and prune the metrics
        /// </summary>
        public void IndexMetrics()
        {
            // Coordinate the time so all metrics are snipped
            // at the same time despite rolling over to a new day etc.
            DateTime now = DateTime.UtcNow; 

            // No need to lock as we are only going to kill items that 
            // are unlikely to be written to (e.g. old items out of the current
            // timeframe)
            foreach (String key in applications.Keys)
                applications[key].IndexMetrics(now);
        }

        /// <summary>
        /// Load the metrics from the cache location (if the service restarted etc.)
        /// </summary>
        /// <param name="loadDirectory"></param>
        /// <returns></returns>
        public Boolean LoadData(String loadDirectory)
        {
            return true;
        }

        /// <summary>
        /// Save the current in-memory metrics to the cache location (so if the service reboots it can be reloaded)
        /// </summary>
        /// <param name="saveDirectory"></param>
        /// <returns></returns>
        public Boolean SaveData(String saveDirectory)
        {
            return true;
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AppMonitorCoordinator()
        {
            // Set up a blank repository for all of the applicatons that interact with this monitor instance
            applications = new Dictionary<string, ReportingApplication>() { };
        }
    }
}
