angular.module('jvmarket')
.controller('AccountCtrl', function ($scope, $rootScope, $location, $window, $http, User) {

   // console.log("AccountCtrl");
    if (!User.isAuthenticated) {
        $location.url("/login");
    }
    $scope.user = User.globals.user;
    $rootScope.section = $rootScope.section || {};
    $rootScope.section.name = "Account";
});