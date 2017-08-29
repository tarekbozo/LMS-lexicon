﻿(function () {
    var app = angular.module("LMSApp");
    app.controller("CoursesController", ["$scope", "$http", function ($scope, $http) {
        var serverBaseUrl = "http://localhost:51942";
        $scope.getData = function () {
            $http.get('http://localhost:51942/api/Course/Get/')
                .then(function (response) {
                    $scope.data = response.data;
                });
        };

    }]);
}());