/**
 * @fileOverview User Service
 */

'use strict';

angular.module('jvmarket')
/**
 * @constructor User
 * @description Contains variables and functions related to the current user their authentication.
 */
.service('Market', function Market($http, $window, $rootScope, $location) {
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

/**
* Reveal a user to another user
* @function
* @name revealUser
* @memberOf Market#
* @param userId - user to reveal to the currently logged in user
* @param callback for completion
*/
    self.revealUser = function (userId, callback) {
        $http({
            method: 'POST',
            url: 'http://jv.com/api/market/revealuser',
            data: { UserId : userId },
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
    /**
* Reveal a program to a user
* @function
* @name revealUser
* @memberOf Market#
* @param programName - program to reveal to the currently logged in user
* @param callback for completion
*/
    self.revealProgram = function (programName, callback) {
        $http({
            method: 'POST',
            url: 'http://jv.com/api/market/revealprogram',
            data: { ProgramName: programName },
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
