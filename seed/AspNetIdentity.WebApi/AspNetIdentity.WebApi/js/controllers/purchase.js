angular.module('jvmarket')
.controller('PurchaseCtrl', function ($scope, $rootScope, $location, $window, $http, User, Market) {
    $rootScope.section = $rootScope.section || {};
    $rootScope.section.name = "Account";
    console.log("PurchaseCtrl");
});