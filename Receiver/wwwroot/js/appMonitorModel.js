var tndStudios = tndStudios || {};
tndStudios.appMonitor = tndStudios.appMonitor || {};
tndStudios.appMonitor.model =
    {
        // Telemetry Page Model
        page: function () {

            // The properties of the object            
            this.applications = new tndStudios.appMonitor.model.applications(null); // The applications object
        },

        // Application Model
        application: function (data) {

            this.title = data;
            this.errors = [];
            this.metrics = [];

            this.addError = function (error) {
                this.errors.push(error);
            }

            this.addMetric = function (metric) {
                this.metrics.push(metric);
            }
        },

        // Applications Model
        applications: function (data) {

            // The list of applications
            this.applicationArray = [];

            // Copy the content of this list of applications from search json package
            this.fromObject = function (fromObject) {

                // Clear the object first (just in case)
                this.clear();

                // Start copying the data from the other object
                this.applicationArray = fromObject;
            }

            this.addApplication = function (applicationName) {
                var found = $.grep(this.applicationArray, function (item) { return item.title == applicationName; });
                if (found.length == 0) {
                    var result = new tndStudios.appMonitor.model.application(applicationName);
                    this.applicationArray.push(result);
                    return result;
                }
                else
                    return found[0];
            }

            // Clear this applications object (i.e. make it ready for use)
            this.clear = function () {

                // Clear the properties
                this.applicationArray = [];

            }

            // Any data passed in?
            if (data) {
                this.fromObject(data); // Assign the data to this object
            }
        },
    };