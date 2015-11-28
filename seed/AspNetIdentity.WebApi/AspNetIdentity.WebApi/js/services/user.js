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
    * Updates a rating for a revealed affiliate
    * @function
    * @name Update Rating
    * @memberOf User#
    * @param {rating} rating level
    * @param {forAffiliate} affiliate being rated
    * @param {callback} callback for completion 
    */
    self.UpdateRating = function (rating, forAffiliate, callback) {
        $http({
            method: 'POST',
            url: 'http://jv.com/api/accounts/rate',
            data: {Rating: rating, AffiliateId: forAffiliate},
            headers: { 'Content-Type': 'application/json' }

        })
      .success(function (data) {

          var msg = "Reting recorded";
          var rsp = { success: true, msg: msg };

          if (angular.isFunction(callback)) {
              callback(rsp);
          }
      }
      ).error(function (err) {
          var msg = "Failed to update rating";

          var rsp = { success: false, msg: msg };
          if (angular.isFunction(callback)) {
              callback(rsp);
          }
      });
    };

    /**
     * Registers a new user
     * @function
     * @name register User
     * @memberOf User#
     * @param {user} token The the user info to use for the registration
     * @param {callback} callback for completion 
     */
    self.registerUser = function (user, callback) {
        $http({
            method: 'POST',
            url: 'http://jv.com/api/accounts/create',
            data: JSON.stringify(user),
            headers: { 'Content-Type': 'application/json' }

        })
       .success(function (data) {

           var msg = "Welcome, your user has been created: " + data.userName;
           var rsp = { success: true, msg: msg };

           if (angular.isFunction(callback)) {
               callback(rsp);
           }
       }
       ).error(function (err) {
           var msg = msgFromModelState(err);

           var rsp = { success: false, msg: msg };
           if (angular.isFunction(callback)) {
               callback(rsp);
           }
       });
    }
    
    /**
     * Log in a user
     * @function
     * @name setAuthToken
     * @memberOf User#
     * @param {string} token The token that should be set for this user to use on all outbound API requests
     */
    self.loginUser = function (user, callback) {
       // var payload = angular.extend({grant_type: "password"}, user);
        $http({
            method: 'POST',
            url: 'http://jv.com/api/accounts/login',
            data: user,
            transformRequest: function (obj) {
                var str = [];
                for (var p in obj)
                    str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                return str.join("&");
            },
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        })
       .success(function (data) {

           var msg = "Welcome ";//+ data.userName;
           var rsp = { success: true, msg: msg };
           self.globals.user = data.user;
           self.isAuthenticated = true;
           self.setAuthToken( data.token );
           if (angular.isFunction(callback)) {
               callback(rsp);
           }
       }
       ).error(function (err) {      
           var msg = msgFromModelState(err);

           var rsp = { success: false, msg: msg };
           if (angular.isFunction(callback)) {
               callback(rsp);
           }
       })
    }

    self.hasCredits = function () {
        if (this.globals.user == null) { return false; }
        if (angular.isObject(this.globals.user) && angular.isString(this.globals.user.Credits) && this.globals.user.Credits == "0") { return false; }
        return true;
    };

    function msgFromModelState(err) {
        // TODO some http response code filtering here
        var msg = null;
        if (err.modelState && err.modelState["createUserModel.Password"] && err.modelState["createUserModel.Password"].length > 0) {
            msg = err.modelState["createUserModel.Password"][0];
        }
        if (msg == null && err.modelState && err.modelState["createUserModel.ConfirmPassword"] && err.modelState["createUserModel.ConfirmPassword"].length > 0) {
            msg = err.modelState["createUserModel.ConfirmPassword"][0];
        }
        if (msg == null && err.modelState && err.modelState["createUserModel.Email"] && err.modelState["createUserModel.Email"].length > 0) {
            msg = err.modelState["createUserModel.Email"][0];
        }
        if (msg == null && err.modelState && err.modelState["createUserModel.FirstName"] && err.modelState["createUserModel.FirstName"].length > 0) {
            msg = err.modelState["createUserModel.FirstName"][0];
        }
        if (msg == null && err.modelState && err.modelState["createUserModel.LastName"] && err.modelState["createUserModel.LastName"].length > 0) {
            msg = err.modelState["createUserModel.LastName"][0];
        }
        if (msg == null && err.modelState && err.modelState["createUserModel.PhoneNumber"] && err.modelState["createUserModel.PhoneNumber"].length > 0) {
            msg = err.modelState["createUserModel.PhoneNumber"][0];
        }
        if (msg == null && err.modelState && err.modelState["createUserModel.SkypeHandle"] && err.modelState["createUserModel.SkypeHandle"].length > 0) {
            msg = err.modelState["createUserModel.SkypeHandle"][0];
        }
        if (msg == null && err.modelState && err.modelState["createUserModel.ProgramUrl"] && err.modelState["createUserModel.ProgramUrl"].length > 0) {
            msg = err.modelState["createUserModel.ProgramUrl"][0];
        }
        if (msg == null && err.modelState && err.modelState["createUserModel.ProgramDescription"] && err.modelState["createUserModel.ProgramDescription"].length > 0) {
            msg = err.modelState["createUserModel.ProgramDescription"][0];
        }
        if (msg == null && err.modelState && err.modelState["createUserModel.ProgramName"] && err.modelState["createUserModel.ProgramName"].length > 0) {
            msg = err.modelState["createUserModel.ProgramName"][0];
        }
        if (msg == null && err.modelState && err.modelState["createUserModel.IndividualDescription"] && err.modelState["createUserModel.IndividualDescription"].length > 0) {
            msg = err.modelState["createUserModel.IndividualDescription"][0];
        }
        if (msg == null && err.modelState && err.modelState["createUserModel.ProgramCategory"] && err.modelState["createUserModel.ProgramCategory"].length > 0) {
            msg = err.modelState["createUserModel.ProgramCategory"][0];
        }
        if (msg == null && err.modelState && err.modelState["createUserModel.UserCategory"] && err.modelState["createUserModel.UserCategory"].length > 0) {
            msg = err.modelState["createUserModel.UserCategory"][0];
        }
        if (err.modelState && err.modelState[0] && angular.isArray(err.modelState[0])) {
            msg = err.modelState[0][0];
        }

        if (err.error)
        {
            msg = error;
        }

        if (msg == null) {
            msg = 'There was a problem';
        }

        return msg;

    }

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
     * Delete the users authentication credentials, notifies server of logout, redirects user back to login page.
     * @function
     * @name logout
     * @memberOf User#
     * @param {Function} callback The function to call upon success or failure
     */
    //self.logout = function (callback) {
    //    // make logout call to server to clear session cookies there
    //    $http.put($rootScope.config.api + "/users/logout", {}, {}).success(function () {
    //        // reset user auth
    //        self.isAuthenticated = false;
    //        $rootScope.userIsAuthenticated = false; // need this because scoping issue prevents nav bars from triggering
    //        self.deleteAuthToken();
    //        if (angular.isFunction(callback)) {
    //            callback(true);
    //        }
    //    })
    //    .error(function () {
    //        // reset user auth
    //        self.isAuthenticated = false;
    //        $rootScope.userIsAuthenticated = false; // need this because scoping issue prevents nav bars from triggering
    //        self.deleteAuthToken();
    //        if (angular.isFunction(callback)) {
    //            callback(false);
    //        }
    //    });

    //    self.username = "";
    //    self.userID = -1;
    //};



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
