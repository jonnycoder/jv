/**
 * @fileOverview User Service
 */

'use strict';

angular.module('jvmarket')
/**
 * @constructor User
 * @description Contains variables and functions related to the current user their authentication.
 */
.service('Market', function User($http, $window, $rootScope, $location) {
    /** @lends User# */
    console.log("Market service");
    var self = this;

    /**
 * Get resources available to a user
 * @function
 * @name getResources
 * @memberOf Market#
 * @param callback for completion
 */
    self.getResources = function (callback) {
        $http({
            method: 'GET',
            url: 'http://jv.com/api/market/resources',       
            headers: { 'Accept': 'application/json' }
        })
       .success(function (data) {
       
           var rsp = { success: true, data: data };
           self.resources = data;
           if (angular.isFunction(callback)) {
               callback(rsp);
           }
       }
       ).error(function (err) {
          // var msg = msgFromModelState(err);

           var rsp = { success: false, err: err };
           if (angular.isFunction(callback)) {
               callback(rsp);
           }
       })
    }
});
