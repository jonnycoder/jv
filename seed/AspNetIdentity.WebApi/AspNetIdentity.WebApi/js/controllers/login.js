angular.module('jvmarket')
.controller('LoginCtrl', function ($scope, $rootScope, $location, $window, $http, $timeout, User) {
    $rootScope.section = $rootScope.section || {};
    $rootScope.section.name = "Login";
    console.log("LoginCtrl");
    User.setAuthToken(null);
    $rootScope.userLoggedIn = false;
    $scope.user = {};
    $scope.submitLoginForm = function () {
        $scope.errorPassword = null;
        $scope.errorEmail = null;
        $scope.errorUserName = null;
        $scope.createConfirm = null;
        User.loginUser($scope.user, function (loginRspModel) {
            if (loginRspModel.success) {
                $rootScope.hasCredits = User.hasCredits();
                $rootScope.$broadcast('event:auth-success', loginRspModel);
                $scope.errorLogin = "Successful login";
                $rootScope.userLoggedIn = true;
                $timeout(function () {
                    $location.url("/market");
                }, 750);
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