angular.module('jvmarket')
.controller('MarketCtrl', function ($scope, $rootScope, $location, $window, $http, $sce, User, Market) {
    $rootScope.section = $rootScope.section || {};
    $rootScope.section.name = "Market";
    console.log("MarketCtrl");
    $scope.user = {};
    $scope.affiliateCategory = "All";
    $scope.programCategory = "All";
    $scope.resources = null;
    $scope.unlockProgramMessages = [];
    $scope.unlockAffiliateMessages = [];
    $scope.percent = [];
    if (!User.isAuthenticated) {
        $location.url("/login");
    }

    $scope.revealUser = function (revealUserId, i) {
        console.log("reveal " + revealUserId);
        $scope.unlockAffiliateMessages[i] = "Checking...";

        Market.revealUser(revealUserId, function (response) {
            if (response.success) {
                // fire a user refresh call
                User.refreshUser(null);

                // update the resources a user has access to
                Market.getResources(function (response) {
                    if (response.success) {
                        $scope.resources.unlockedAffiliates = response.data.unlockedAffiliates;
                        $scope.unlockAffiliateMessages[i] = "Details for affiliate revealed, please check the UNLOCKED tab for updates";
                        $scope.GenerateTabsForResources(0);
                    }

                });
            } else {
                $scope.unlockAffiliateMessages[i] = "Cannot reveal this affiliate at this time";
                if (angular.isObject(User.globals.user) && User.globals.user.Credits === "0") {
                    $scope.unlockAffiliateMessages[i] += " you have 0 remaining credits";
                }
            }
        });
    }

    $scope.revealProgram = function (revealProgramName, i) {
        console.log("reveal " + revealProgramName);
        $scope.unlockProgramMessages[i] = "Checking...";


        Market.revealProgram(revealProgramName, function (response) {
            if (response.success) {
                // fire a user refresh call
                User.refreshUser(null);
                Market.getResources(function (response) {
                    if (response.success) {
                        $scope.resources.unlockedPrograms = response.data.unlockedPrograms;
                        $scope.unlockProgramMessages[i] = "Details for " + revealProgramName + " revealed, please check the UNLOCKED tab for updates";
                        $scope.GenerateTabsForResources(1);
                    }
                });
            }
            else {
                $scope.unlockProgramMessages[i] = "Cannot reveal this program at this time";
                if (angular.isObject(User.globals.user) && User.globals.user.Credits === "0") {
                    $scope.unlockProgramMessages[i] += " you have 0 remaining credits";
                }
            }
        });
     
    }

    // $scope.rate = 7;
    $scope.max = 5;
    $scope.isReadonly = false;
    $scope.qualities = ['poor', 'mediocore', 'ok', 'quite good', 'excellent'];
    $scope.hoveringOver = function (value, index) {
        $scope.overStar = value;
        $scope.percent[index] = 100 * (value / $scope.max);
        $scope.quality = $scope.qualities[($scope.percent / 20) - 1];
    };

    $scope.updateRating = function (rating, forAffiliate) {
        console.log("rating " + rating + " for affiliate " + forAffiliate);
        User.UpdateRating(rating, forAffiliate, function (response) {
            // not much to do here on success or failure
        });
    }

    $scope.GenerateTabsForResources = function (selected) {
        $scope.panes = [];

        if ($scope.resources && $scope.resources.affiliates && $scope.resources.affiliates.length > 0) {
            $scope.panes.push({ icon: $sce.trustAsHtml("<i class=\"icon-user\"></i>&nbsp;&nbsp;AFFILIATES"), content: "/js/views/affiliates.html", active: selected == 0 });

            $scope.resources.affiliates.forEach(function (element, index, array) {
                $scope.unlockAffiliateMessages.push("");
            });
        }

        if ($scope.resources && $scope.resources.programs && $scope.resources.programs.length > 0) {
            $scope.panes.push({ icon: $sce.trustAsHtml("<i class=\"icon-exchange\"></i>&nbsp;&nbsp;PROGRAMS"), content: "/js/views/programs.html", active: ($scope.panes.length == 0 || selected == 1) });

            $scope.resources.programs.forEach(function (element, index, array) {
                $scope.unlockProgramMessages.push("");
            });
        }

        if ($scope.resources &&
            (($scope.resources.unlockedAffiliates && $scope.resources.unlockedAffiliates.length > 0) ||
              ($scope.resources.unlockedPrograms && $scope.resources.unlockedPrograms.length > 0))) {
            $scope.panes.push({ title: "UNLOCKED", icon: $sce.trustAsHtml("<i class=\"icon-unlock\"></i>&nbsp;&nbsp;UNLOCKED"), content: "/js/views/unlocked.html", active: false });
        }
    }

    Market.getResources(function (response) {
        if (response.success) {
            $scope.resources = response.data;
            $scope.resources.filteredAffiliates = $scope.resources.affiliates;
            $scope.resources.filteredPrograms = $scope.resources.programs;
            $scope.GenerateTabsForResources(0);
        }
    });

    $scope.filterAffiliates = function (filterScope) {
        var filterValue = filterScope.affiliateCategory;
        if (filterValue === "All") {
            $scope.resources.filteredAffiliates = $scope.resources.affiliates;
            return;
        }

        var cutList = [];
        $scope.resources.affiliates.forEach(function (item, index, collection) {
            if (item.CategoryDescription === filterValue) {
                cutList.push(item);
            }
        });

        $scope.resources.filteredAffiliates = cutList;
    };

    $scope.filterPrograms = function (filterScope) {
        var filterValue = filterScope.programCategory;
        if (filterValue === "All") {
            $scope.resources.filteredPrograms = $scope.resources.programs;
            return;
        }

        var cutList = [];
        $scope.resources.programs.forEach(function (item, index, collection) {
            if (item.ProgramCategory === filterValue) {
                cutList.push(item);
            }
        });

        $scope.resources.filteredPrograms = cutList;
    };
});