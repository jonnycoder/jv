angular.module('jvmarket')
.controller('LoginCtrl', function ($scope, $rootScope, $location, $window, $http, User) {
    $rootScope.section = $rootScope.section || {};
    $rootScope.section.name = "Login";
    console.log("LoginCtrl");
    User.setAuthToken(null);
    $scope.user = {};
    $scope.submitLoginForm = function () {
        $scope.errorPassword = null;
        $scope.errorEmail = null;
        $scope.errorUserName = null;
        $scope.createConfirm = null;
        User.loginUser($scope.user, function (loginRspModel) {
            if (loginRspModel.success) {
                $rootScope.$broadcast('event:auth-success', loginRspModel);
                $location.url("/market");
                $scope.errorLogin = "Successful login";
            }
            else {
                $scope.errorLogin = loginRspModel.msg;
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