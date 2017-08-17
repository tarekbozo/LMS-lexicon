(function(){
var app = angular.module("LMSApp");
    app.controller("HomeController", ["$scope", "$http", function ($scope, $http) {

        $scope.getData = function () {
            $http.get('/api/HomeAPI/Get/')
                .then(function (response) {
                    $scope.data = response.data;
                })
        };
    }]);

}());