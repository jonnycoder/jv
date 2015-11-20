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

    $scope.reveal = function (revealUserId) {
        console.log("reveal " + revealUserId);
    }

    Market.getResources(function (response) {
        if (response.success) {
            $scope.resources = response.data;

            $scope.panes = [];

            if ($scope.resources && $scope.resources.affiliates && $scope.resources.affiliates.length > 0 ) {
                $scope.panes.push({ icon: $sce.trustAsHtml("<i class=\"icon-user\"></i>&nbsp;&nbsp;AFFILIATES"), content: "/js/views/affiliates.html", active: true });
            }

            if ($scope.resources && $scope.resources.programs && $scope.resources.programs.length > 0) {
                $scope.panes.push({ icon: $sce.trustAsHtml("<i class=\"icon-exchange\"></i>&nbsp;&nbsp;PROGRAMS"), content: "/js/views/programs.html", active: ($scope.panes.length == 0) });
            }

            if ($scope.resources &&
                (($scope.resources.unlockedAffiliates && $scope.resources.unlockedAffiliates.length > 0) || 
                  ($scope.resources.unlockedPrograms && $scope.resources.unlockedPrograms.length > 0))) {
                $scope.panes.push({ title:"UNLOCKED", icon: $sce.trustAsHtml("<i class=\"icon-unlock\"></i>&nbsp;&nbsp;UNLOCKED"), content: "/js/views/unlocked.html", active: false });
            }
        }
    });

});