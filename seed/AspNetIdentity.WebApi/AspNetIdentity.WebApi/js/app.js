angular.module('jvmarket', [
                                'ngRoute',
                                'angularMoment',
                                'ngAnimate',
                                'ui.bootstrap'
])
.config(function ($routeProvider, $httpProvider, $compileProvider) {
    $httpProvider.interceptors.push('authInterceptor');

    $routeProvider
    .when("/", {
        controller: "RegisterCtrl",
        templateUrl: "js/views/register.html"
    }).when("/login", {
        controller: "LoginCtrl",
        templateUrl: "js/views/login.html"
    }).when("/market", {
        controller: "MarketCtrl",
        templateUrl: "js/views/market.html"
    }).when("/account", {
        controller: "AccountCtrl",
        templateUrl: "js/views/Account.html"
    }).when("/account/purchase", {
        controller: "PurchaseCtrl",
        templateUrl: "js/views/Purchase.html"
    }).when("/about", {
        controller: "InfoCtrl",
        templateUrl: "js/views/About.html"
    }).when("/contact", {
        controller: "InfoCtrl",
        templateUrl: "js/views/Contact.html"
    });
})
.run(function ($rootScope, $document, $window, $location, User) {
    $rootScope.appConfig = {
        appname: "JV Matchup",
        url: "jv.com",
        api: "jv.com/api",
        version: "0.5"
    };
    
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