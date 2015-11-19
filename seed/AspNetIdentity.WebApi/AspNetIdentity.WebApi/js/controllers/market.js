angular.module('jvmarket')
.controller('MarketCtrl', function ($scope, $rootScope, $location, $window, $http, $sce, User, Market) {
    $rootScope.section = $rootScope.section || {};
    $rootScope.section.name = "Login";
    console.log("MarketCtrl");
    $scope.user = {};
    $scope.resources = null;

    if (!User.isAuthenticated) {
        $location.url("/login");
    }

    Market.getResources(function (response) {
        if (response.success) {
            $scope.resources = response.data;

            $scope.panes = [];

            if ($scope.resources && $scope.resources.affiliates && $scope.resources.affiliates.length > 0 ) {
                $scope.panes.push(   { title: "AFFILIATES", icon: "", content: "/js/views/affiliates.html", active: true });  
            }

            if ($scope.resources && $scope.resources.programs && $scope.resources.programs.length > 0) {
                $scope.panes.push({ title: "PROGRAMS", icon:"", content: "/js/views/programs.html", active: ($scope.panes.length == 0) });
            }

            if ($scope.resources &&
                (($scope.resources.unlockedAffiliates && $scope.resources.unlockedAffiliates.length > 0) || 
                  ($scope.resources.unlockedPrograms && $scope.resources.unlockedPrograms.length > 0))) {
                $scope.panes.push({ title:"UNLOCKED", content: "/js/views/unlocked.html", active: false });
            }
        }
    });

});