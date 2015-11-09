/**
 * @fileOverview User Service
 */

'use strict';

angular.module('jvmarket')
/**
 * @constructor User
 * @description Contains variables and functions related to the current user their authentication.
 */
.service('User', function User($http, $window, $rootScope, $location) {
    /** @lends User# */
    console.log("User service");
    var self = this;

    /** Whether or not this user has logged in */
    self.isAuthenticated = false;

    /** The current user's username in nerv */
    self.username = "";
    self.email = "";

    /** Global alerts object used to manage badge totals in the dashboard, etc */
    self.globals = {
        unreadMessages: 0,
        profileCompleteness: 0,
        forms: {
            incomplete: 0,
            total: 0
        },
        location: {
            id: "",
            name: ""
        }
    };

    /** The users default time zone */
    self.timeZone = "";

    /** The current user's ID in nerv */
    self.userID = -1;

    /** The current user's name */
    self.firstName = '';
    self.lastName = '';
    self.middleName = '';

    self.avatarUrl = '';

    //var authPromise = $q.defer();
    //self.authPromise = function () {
    //    return authPromise.promise;
    //};

    self.fullName = function () {
        return self.firstName + ' ' + self.lastName;
    };

    self.middleInitial = function () {
        return self.middleName ? self.middleName.substr(0, 1) : null;
    };

    /**
     * Sets the current optic api authentication token
     * @function
     * @name setAuthToken
     * @memberOf User#
     * @param {string} token The token that should be set for this user to use on all outbound API requests
     */
    self.setAuthToken = function (token) {
        $window.localStorage.authToken = token;
    };

    /**
     * Deletes the current optic api authentication token.
     * @function
     * @name deleteAuthToken
     * @memberOf User#
     */
    self.deleteAuthToken = function () {
        $window.localStorage.authToken = "";
    };

    /**
     * Called on login and token check to refresh a user object based on data coming back from API
     * @function
     * @name populateUserObject
     * @memberOf User#
     * @param {Object} data API user object with, translate to local model
     */
    function populateUserObject(data) {
        self.username = data.Username;
        self.userID = data.UserId;
        self.firstName = data.FirstName;
        self.lastName = data.LastName;
        self.middleName = data.MiddleName;
        self.email = data.EmailAddress;


        if (angular.isArray(data.Patients) && data.Patients.length > 0) {
            Patient.initialize(data.Patients[0]);
        }

        if (angular.isString(data.ImageUrl)) {
            self.avatarUrl = data.ImageUrl.replace(/\?sz=[0-9]+/, "");
        }

        // TODO: User object will be updated in API to deliver more app related items
        // such as profile completeness, unread messages, incomplete forms, etc
        // so that we have that data immediately visible to the user
        self.globals.unreadMessages = data.UnreadMessages;
        self.globals.profileCompleteness = Math.floor(data.ProfileCompletenessPercent);
        self.globals.forms.incomplete = data.IncompleteIntakeForms;
        self.globals.forms.total = data.IntakeFormTotal;
        self.globals.location = {
            id: "1234",
            name: "My Location"
        };

        self.timeZone = "-0500"; // default timezone, will be loaded on login later
    }

    /**
     * Attempt to log this user in with the given credentials
     * @function
     * @name login
     * @memberOf User#
     * @param {String} username The email to login as
     * @param {String} password The password to attempt
     * @param {Object} captcha Any captcha information to send up, if applicable
     * @param {Function} callback The function to call upon success or failure
     */
    self.login = function (username, password, captcha, callback) {
        self.isAuthenticated = false;
        self.username = "";
        self.userID = -1;

        var results = {
            success: false,
            message: ""
        };

        // captcha is optional, so make sure its at least an object before going forward
        if (!angular.isObject(captcha)) {
            captcha = {};
        }

        // form the request data
        var reqData = {
            Username: username,
            Password: password,
            CaptchaChallenge: captcha.challenge,
            CaptchaResponse: captcha.response
        };

        // go ahead and log them out
        self.deleteAuthToken();

        // make api call here w/ $http
        $http.post($rootScope.config.api + "/users/login", jQuery.param(reqData), { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
        .success(function (data) {
            self.isAuthenticated = true;

            // set the auth token
            self.setAuthToken(data.AuthenticationToken);
            populateUserObject(data);
            authPromise.resolve(self);

            results.success = true;
            results.message = "Logged in successfully";

            if (angular.isFunction(callback)) {
                callback(results);
            }
        })
        .error(function (data) {
            self.isAuthenticated = false;

            authPromise.reject(self);

            results.success = false;
            if (angular.isObject(data)) {
                results.message = data.FriendlyMessage;
                results.DataStore = data.DataStore;
                results.Property = data.Property;
            }
            else {
                results.message = "Error logging in. Please try again.";
            }

            if (angular.isFunction(callback)) {
                callback(results);
            }
        });
    };

    // request

    /**
     * Delete the users authentication credentials, notifies server of logout, redirects user back to login page.
     * @function
     * @name logout
     * @memberOf User#
     * @param {Function} callback The function to call upon success or failure
     */
    self.logout = function (callback) {
        // make logout call to server to clear session cookies there
        $http.put($rootScope.config.api + "/users/logout", {}, {}).success(function () {
            // reset user auth
            self.isAuthenticated = false;
            $rootScope.userIsAuthenticated = false; // need this because scoping issue prevents nav bars from triggering
            self.deleteAuthToken();
            if (angular.isFunction(callback)) {
                callback(true);
            }
        })
        .error(function () {
            // reset user auth
            self.isAuthenticated = false;
            $rootScope.userIsAuthenticated = false; // need this because scoping issue prevents nav bars from triggering
            self.deleteAuthToken();
            if (angular.isFunction(callback)) {
                callback(false);
            }
        });

        self.username = "";
        self.userID = -1;
    };

    self.resetDevice = function () {
        self.isAuthenticated = false;
        self.deleteAuthToken();
        self.setMobileDeviceAuthCode("");

        $window.localStorage.removeItem("clientName");
        $window.localStorage.removeItem("clientId");
        $window.localStorage.removeItem("clientSubdomain");
        $window.localStorage.removeItem("userName");
    };

    self.hasAuthenticatedDevice = function () {
        var authKey = self.getMobileDeviceAuthCode();
        return angular.isString(authKey) && authKey !== "" &&
        angular.isString($window.localStorage.userName) && $window.localStorage.userName !== "";
    };

    self.setMobileDeviceAuthCode = function (code) {
        $window.localStorage.authenticationKey = code;
    }
    self.getMobileDeviceAuthCode = function () {
        return $window.localStorage.authenticationKey;
    };
    self.getDeviceUUID = function () {
        return angular.isObject($rootScope.deviceSettings) ? $rootScope.deviceSettings.uuid : "";
    };
   

    self.mobileLogin = function (pin, callback) {
        var uuid = self.getDeviceUUID();
        var hash = self.getMobileDeviceAuthCode();

        self.deleteAuthToken();

        $http.post($rootScope.config.api + "/users/mobile/devices/login", {
            'DeviceUUID': uuid,
            'DeviceHash': hash,
            'PinCode': pin
        }).success(function (data) {
            if (data.AccountLocked || !data.Enabled) {
                var err = { HttpStatusCode: 403, FriendlyMessage: "This account is disabled or has been locked out" };

                if (angular.isFunction(callback)) {
                    callback({ success: false, error: err });
                    return;
                }
            }

            self.isAuthenticated = true;
            $rootScope.userIsAuthenticated = true;


            $window.localStorage.setItem("userName", data.FirstName);

            // set the auth token
            self.setAuthToken(data.AuthenticationToken);
            populateUserObject(data);
            authPromise.resolve(self);

            if (angular.isFunction(callback)) {
                callback({ success: true, data: data });
            }

        }).error(function (err) {

            if (angular.isObject(err) && angular.isObject(err.DataStore)) {
                $rootScope.forceUpgradeModel = err.DataStore;
            }

            if (angular.isFunction(callback)) {
                callback({ success: false, error: err });
            }
        });
    };

});
