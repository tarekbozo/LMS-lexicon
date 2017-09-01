(function () {
    var app = angular.module("LMSApp");
    app.controller("HomeController", ["$scope", "$http", function ($scope, $http) {
        var serverBaseUrl = "http://localhost:51942";
        if (sessionStorage.getItem("isLoggedIn") != null && sessionStorage.getItem("isLoggedIn") == true && sessionStorage.getItem("isLoggedIn") != undefined)
            $scope.getData = function () {
                $http.get('http://localhost:51942/api/Home/Get/')
                    .then(function (response) {
                        $scope.data = response.data;
                    });
            };
        $scope.DownloadExcelFile = function () {
            window.open('http://localhost:51942/api/Home/GetXLSFile/');
        };
        function getHeaders() {
            return { "Authorization": "Bearer " + sessionStorage.getItem("token") }
        }
    }]);
}());