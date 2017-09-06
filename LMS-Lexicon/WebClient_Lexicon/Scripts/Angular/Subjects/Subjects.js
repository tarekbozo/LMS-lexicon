(function () {
    var app = angular.module("LMSApp");
    var serverBaseUrl = "http://localhost:51942";

    //index
    app.controller("Subjects_IndexController", ["$scope", "$http", function ($scope, $http) {
        $scope.reverse = false;
        $scope.myOrderBy = 'Name';

        $scope.orderByMe = function (type) {
            $scope.reverse = ($scope.myOrderBy === type) ? !$scope.reverse : false;
            $scope.myOrderBy = type;
        };
        $scope.InitialSubjects = function () {
            $http.get(serverBaseUrl+'/api/Subjects/Get')
                .then(function (response) {
                    $scope.subjects = JSON.parse(JSON.stringify(response.data));
                });
        };

    }]);
}());