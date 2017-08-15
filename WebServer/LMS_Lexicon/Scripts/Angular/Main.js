(function () {
    var app = angular.module("LMSApp", []);
    app.controller("MainController", ["$scope", "$http", function ($scope, $http) {

        $scope.getData = function () {
            $http.get('/api/Values/Get/')
                .then(function (response) {
                    $scope.data = response.data;
                })
        };
    }]);
}());