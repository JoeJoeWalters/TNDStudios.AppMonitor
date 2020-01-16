using System;
using System.Collections.Generic;
using TNDStudios.AppMonitor.Objects;

namespace TNDStudios.AppMonitor.Core
{
    public interface IAppMonitorCore
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
    }

    public class AppMonitorCore : IAppMonitorCore
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
            return new ReportingSummary() { };
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
        /// Default Constructor
        /// </summary>
        public AppMonitorCore()
        {
            // Set up a blank repository for all of the applicatons that interact with this monitor instance
            applications = new Dictionary<string, ReportingApplication>() { };
        }
    }
}
