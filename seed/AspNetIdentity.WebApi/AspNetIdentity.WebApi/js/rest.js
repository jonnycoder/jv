/** 
* @constructor AuthInterceptor 
* @description Methods related to outbound requests and their results
*/
angular.module('jvmarket')
.factory('authInterceptor', function($rootScope, $q, $window){

    return {
        /**
        * On outbound requests to the API, inject our Authentication Token
        * @function
        * @name request
        * @memberOf AuthInterceptor#
        * @param {Object} config Any current configuration settings on this outbound request
        * @returns {Object} Updated config to include our AuthToken, if applicable
        */
        request: function(config){
            // if this is an api request, tack on our auth token
            if (config.url.indexOf($rootScope.appConfig.api) === 0){
                config.headers = config.headers || {};

                if (angular.isString($window.localStorage.authToken) && $window.localStorage.authToken !== ""){
                    config.headers['Authorization'] = "Bearer " +  $window.localStorage.authToken;
                }
         
                // send up local version number converted from the double decimal notation to single: so
                //   0.5.1 will be converted to 0.51 before it's transmitted to the API
                var decimalVersion = "0.1";
                if (angular.isString($rootScope.appConfig.version)) {
                    decimalVersion = $rootScope.appConfig.version.replace(/([0-9]\.[0-9])(\.)([0-9])/, "$1$3");
                }
       
            }

            return config;
        },
        /**
        * Called whenever an error response is received from the API. If a 401 (Unauthorized) request is received,
        * redirect this user to the Login page.
        * @function
        * @name responseError
        * @memberOf AuthInterceptor#
        * @param {Object} response The response object that was returned from the request that was made.
        * @returns {Object} Deferred or rejected promise
        */
        responseError: function(response){
            // paths to be excluded from this 401 redirect
            //var excludePaths = [
            //    $rootScope.appConfig.api+"/users/login"
            //];

            //if (response.status === 401 && excludePaths.indexOf(response.config.url) < 0) {
            //    var deferred = $q.defer();
                 
            //    $rootScope.reAuthenticate();
         
            //    return deferred.promise;
            //}
         
            //if (response.status === 423) {
            //    console.log("interceptor response, got the 423", response.data);
            //    var deferred = $q.defer();
            //}

            //// otherwise, default behaviour
            //return $q.reject(response);
            return response;
        }
    };
});