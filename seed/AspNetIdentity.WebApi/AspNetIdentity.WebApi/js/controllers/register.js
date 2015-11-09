angular.module('jvmarket')
.controller('RegisterCtrl', function ($scope, $rootScope, $location, $window, $http, User) {
    $rootScope.section = $rootScope.section || {};
    $rootScope.section.name = "Register";
    console.log("RegisterCtrl");
    $scope.user = {};
    $scope.submitRegisterForm = function () {
        $scope.errorPassword = null;
        $scope.errorEmail = null;
        $scope.errorUserName = null;
        $scope.createConfirm = null;
        User.registerUser($scope.user, function (registerRspModel) {
            if (registerRspModel.success) {
                $scope.createConfirm = registerRspModel.msg;
            }
            else {
                $scope.errorPassword = registerRspModel.msg;
            }
        });
    };

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