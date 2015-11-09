angular.module('jvmarket', [
                                'ngRoute',
                                'angularMoment'
])
.config(function ($routeProvider, $httpProvider, $compileProvider) {
    $httpProvider.interceptors.push('authInterceptor');

    $routeProvider
    .when("/", {
        controller: "RegisterCtrl",
        templateUrl: "js/views/register.html"
    });
})
.run(function ($rootScope, $document, $window, $location, User) {
    $rootScope.appConfig = {
        appname: "JV Market",
        url: "jv.com",
        api: "jv.com/api",
        version: "0.5"
    };

    //$rootScope.section = {
    //    name: ""
    //};

    //$rootScope.$on("$routeChangeStart", function (evt, next, current) {
    //    if (angular.isObject(next) && angular.isObject(next.$$route)) {

    //        var sections = next.$$route.originalPath.split('/');
    //        $rootScope.currentSection = "dashboard";
    //        if (sections.length > 1 && sections[1] !== "") {
    //            $rootScope.currentSection = sections[1].replace(/s$/, "");
    //        }

    //        if (next.$$route.controller === "MailCtrl") {
    //            $rootScope.actionIcon = "newMsg";
    //        }
    //        else {
    //            $rootScope.actionIcon = "newAppt"; // default back to this on any route change
    //        }
    //    }
    //});


    // retrieve client details for this app instance
    $rootScope.hasToken = function () {
       
        var test = $window.localStorage.getItem("authToken") != null;
        return test;
       
    };

      

    $rootScope.logout = function () {
        $rootScope.toggle('mainSidebar', 'off');
        $location.path("auth");
        User.logout(function () { });
    };

    $rootScope.redirect = function (url) {
        $location.path(url);
    };

    $rootScope.reAuthenticate = function () {
        //$rootScope.userIsAuthenticated = false;

        //User.isAuthenticated = false;
        //User.deleteAuthToken();

        //$rootScope.redirect("auth");
    };

    $rootScope.getLocalStorage = function (key) {
        return $window.localStorage.getItem(key);
    };

    $rootScope.setLocalStorage = function (key, obj) {
        $window.localStorage.setItem(key, obj);
    }

});