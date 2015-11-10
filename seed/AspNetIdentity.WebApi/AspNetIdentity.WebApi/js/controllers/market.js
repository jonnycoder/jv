angular.module('jvmarket')
.controller('MarketCtrl', function ($scope, $rootScope, $location, $window, $http, User, Market) {
    $rootScope.section = $rootScope.section || {};
    $rootScope.section.name = "Login";
    console.log("MarketCtrl");
    $scope.user = {};
   

    //User.authPromise().then(function(){
    //    mixpanel.track("View Dashboard");
    //    $scope.accountMsg = "Logged in as: " + User.email;
    //    Appointments.getAppointments(false).then(
    //        function (results) {
    //            $scope.upcomingAppts = results;
    //        },
    //        function (error) {
    //            console.log("error getting upcoming appointments");
    //        });
    //});

    //$scope.bookNewAppt = function bookNewAppt() {
    //    $location.path("/appts/new");
    //};

    //$scope.gotoMeds = function gotoMeds() {
    //    $location.path("/meds");
    //};

    //$scope.viewAppt = function viewAppt(appt) {
    //    $rootScope.currentAppointment = appt;
    //    $location.path("/appt/view");
    //};

});