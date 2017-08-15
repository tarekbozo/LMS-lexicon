app.controller("HomeController", ["$scope", "$http", function ($scope, $http) {

    $scope.getData = function () {
        $http.get('http://localhost:52255/api/HomeAPI/Get/')
            .then(function (response) {
                $scope.data = response.data;
            })
    };
}]);