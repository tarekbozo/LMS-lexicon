(function(){
var app = angular.module("LMSApp");
    app.controller("HomeController", ["$scope", "$http", function ($scope, $http) {
        var serverBaseUrl = "http://localhost:51942";
        $scope.getData = function () {
            $http.get('http://localhost:51942/api/Home/Get/')
                .then(function (response) {
                    $scope.data = response.data;
                });
        };
        $scope.DownloadExcelFile = function () {
            window.open('http://localhost:51942/api/Home/GetXLSFile/');
        };
        //$scope.GetUser = function () {
        //    var _url = serverBaseUrl + "/api/UsersAPI/GetUserInfoFromCurrentUser?userName="+sessionStorage.getItem("username");
        //    $http({
        //        method: 'GET',
        //        url: _url,
        //        headers: getHeaders(),
        //    }).then(function (response) {
        //        $scope.user = response.data;
        //    })
        //}

        function getHeaders() {
                return { "Authorization": "Bearer " + sessionStorage.getItem("token") }
        }
    }]);
}());
