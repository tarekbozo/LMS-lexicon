(function () {
    var app = angular.module("LMSApp");
    app.controller("HomeController", ["$scope", "$http", function ($scope, $http) {
        var serverBaseUrl = "http://localhost:51942";
            $scope.getData = function () {
                $http.get('http://localhost:51942/api/Home/Get/')
                    .then(function (response) {
                        $scope.data = response.data;
                    });
            };
            $scope.fileUpdate = function () {
                alert(document.getElementById("MyFile").value);
                var config = { headers: getHeaders() };

                $http.post('http://localhost:51942/api/Home/SendFile/', document.getElementById("MyFile").value, config)
                    .then(function (response) {
                        $scope.data = response.data;
                    });
            }
        $scope.DownloadExcelFile = function () {
            window.open('http://localhost:51942/api/Home/GetXLSFile/');
        };
        function getHeaders() {
            return { "Authorization": "Bearer " + sessionStorage.getItem("token") }
        }
    }]);
}());