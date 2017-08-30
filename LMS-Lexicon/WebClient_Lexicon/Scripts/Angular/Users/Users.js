(function () {
    var app = angular.module("LMSApp");
    app.controller("UsersCtrl", ["$scope", "$http", function ($scope, $http) {
        var serverBaseUrl = "http://localhost:51942";

        $scope.GetUsers = function () {
            var _url = serverBaseUrl + "/api/UsersAPI/GetUsers";
            $http({
                method: 'GET',
                url: _url,
                headers: getHeaders(),
            }).then(function (response) {
                $scope.users = response.data;
            })
        }

        function getHeaders() {
            return { "Authorization": "Bearer " + sessionStorage.getItem("token") }
        }
    }]);
}());