angular.module('jvmarket')
.controller('RegisterCtrl', function($scope, $rootScope, $location, $window, $http, User) {
    $rootScope.section = $rootScope.section || {};
    $rootScope.section.name = "Register";
    console.log("RegisterCtrl");
    $scope.user = {};
    $scope.submitRegisterForm = function () {
        $scope.errorPassword = null;
        $scope.errorEmail = null;
        $scope.errorUserName = null;
        $scope.createConfirm = null;
        $http({
            method: 'POST',
            url: 'http://jv.com/api/accounts/create',
            data: JSON.stringify($scope.user),
            headers: { 'Content-Type': 'application/json' }

        })
        .success(function (data) {
            $scope.createConfirm = "Welcome, your user has been created: " + data.userName;
        }
        ).error(function (err) {
            if (err.modelState["createUserModel.Password"] && err.modelState["createUserModel.Password"].length > 0) {
                $scope.errorPassword = err.modelState["createUserModel.Password"][0];
                // TODO more model state errors out to the UI here
            }

            if (err.modelState[0] && angular.isArray(err.modelState[0])) {
                var msg = err.modelState[0][0];
                $scope.errorPassword = msg;
            }
        })
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