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
});