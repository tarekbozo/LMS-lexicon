(function () {
    var app = angular.module("LMSApp");
    app.controller("GradesAPIController", ["$scope", "$http", function ($scope, $http) {
        var serverBaseUrl = "http://localhost:51942";
        $scope.getData = function () {
            $http.get('http://localhost:51942/api/Grade/Get/')
                .then(function (response) {
                    $scope.data = response.data;
                });
        };

    }]);
}());
