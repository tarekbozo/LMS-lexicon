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
    //Create,Delete,Edit
    app.controller("Subjects_CreateDeleteEditController", ["$scope", "$http", function ($scope, $http) {
        var subject = {
            ID: null,
            Name: ""
        }
        $scope.Details = function () {
            var n = window.location.href.lastIndexOf('/');
            $http.get(serverBaseUrl + '/api/Subjects/Get?ID=' + window.location.href.substring(n + 1))
            .then(function (response) {
                $scope.subject = response.data;
                subject.ID = response.data.ID;
            }).then(function (err) {
                alert(err.status);
                if (err.status == 400) {
                    $scope.ErrorMessage = "Error: Couldn't find Subject with id: " + window.location.href.substring(n + 1);
                }
            })
        }

        $scope.CreateSubject = function () {
            subject.Name = $scope.subjectName;
            $http({
                method: 'POST',
                url: serverBaseUrl + '/api/Subjects/Create',
                data: subject,
            }).then(function (response) {
                window.location.href = "/Subjects";
            }).then(function (err) {
                if (err.status == 400) {
                    $scope.ErrorMessage = "Error: 400 BadRequest, Subject already exists.";
                }
                else
                {
                    $scope.ErrorMessage = "Error 500, Internal Server error. Please contact Admin";
                }
            })
        }
        $scope.Delete = function () {
            $http({
                method: 'DELETE',
                url: serverBaseUrl + '/api/Subjects/Delete/',
                data: JSON.stringify(subject),
                headers: { "Authorization": "Bearer " + sessionStorage.getItem("token"), 'Content-Type': 'application/json' }

            }).then(function (response) {
                window.location.href = "/Subjects";
            }).then(function (error) {
                $scope.ErrorMessage = "Error: Couldn't delete subject with ID: " + subject.ID;
            })
        }
        $scope.Edit = function () {
            subject.ID = $scope.subject.ID;
            subject.Name = $scope.subjectName;
            $http({
                method: 'Put',
                url: serverBaseUrl + '/api/Subjects/Edit/',
                data: JSON.stringify(subject),
                headers: { "Authorization": "Bearer " + sessionStorage.getItem("token"), 'Content-Type': 'application/json' }

            }).then(function (response) {
                window.location.href = "/Subjects";
            }).then(function (error) {
                $scope.ErrorMessage = "Error: Couldn't Edit subject";
            })
        }

    }]);
}());