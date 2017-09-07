(function () {
    var app = angular.module("LMSApp");
    var serverBaseUrl = "http://localhost:51942";

    //index
    app.controller("Courses_Index_Ctrl", ["$scope", "$http", function ($scope, $http) {
        $scope.reverse = false;
        $scope.myOrderBy = 'Subject.Name';

        $scope.orderByMe = function (type) {
            $scope.reverse = ($scope.myOrderBy === type) ? !$scope.reverse : false;
            $scope.myOrderBy = type;
        };
        $scope.getData = function () {
            var option = { headers: getHeaders() };
            $http.get(serverBaseUrl+'/api/Courses/Get',option)
                .then(function (response) {
                    $scope.courses = JSON.parse(JSON.stringify(response.data));
                })
        };
    }]);
    //Edit
    app.controller("Courses_Edit_Ctrl", ["$scope", "$http", function ($scope, $http) {
        $scope.reverse = false;

        $scope.orderByMe = function (type) {
            $scope.reverse = ($scope.myOrderBy === type) ? !$scope.reverse : false;
            $scope.myOrderBy = type;
        };

        $scope.Start = function () {
            var option = { headers: getHeaders() };
            var n = window.location.href.lastIndexOf('/');
            $http.get(serverBaseUrl + '/api/Courses/Get?ID=' + window.location.href.substring(n + 1),option)
            .then(function (response) {
                $scope.course = response.data;

                $http.get(serverBaseUrl + '/api/UsersAPI/GetAvailableTeachers?subjectID=' + $scope.course.Subject.ID, option)
                    .then(function (response) {
                        $scope.teachers = JSON.parse(JSON.stringify(response.data));
                        $scope.teacher = $scope.teachers[0].Id;
                    });
            })
        }

        $scope.Edit = function () {
            var c = {
                ID: $scope.course.ID,
                SubjectID: $scope.course.Subject.ID,
                TeacherID: $scope.teacher
            };

            $http({
                method: 'PATCH',
                url: serverBaseUrl + '/api/Courses/Edit/',
                data: JSON.stringify(c),
                headers: { "Authorization": "Bearer " + sessionStorage.getItem("token"), 'Content-Type': 'application/json' }

            }).then(function (response) {
                window.location.href = "/Courses";
            }).then(function (error) {
                $scope.ErrorMessage = "Error: Couldn't Edit course";
            })
        }

    }]);
    //Delete
    app.controller("Courses_Delete_Ctrl", ["$scope", "$http", function ($scope, $http) {
        var option = { headers: getHeaders() };
        $scope.Start = function () {
            var n = window.location.href.lastIndexOf('/');
            $http.get(serverBaseUrl + '/api/Courses/Get?ID=' + window.location.href.substring(n + 1),option)
            .then(function (response) {
                $scope.course = response.data;
            })
        }
        $scope.Delete = function () {
            $http({
                method: 'DELETE',
                url: serverBaseUrl + '/api/Courses/Delete/',
                data: JSON.stringify($scope.course),
                headers: { "Authorization": "Bearer " + sessionStorage.getItem("token"), 'Content-Type': 'application/json' }

            }).then(function (response) {
                window.location.href = "/Courses";
            }).then(function (error) {
                $scope.ErrorMessage = "Error: Couldn't delete course with ID: " + $scope.course.ID;
            })
        }
    }]);
    //Details
    app.controller("Courses_Details_Ctrl", ["$scope", "$http", function ($scope, $http) {
        var option = { headers: getHeaders() };
        $scope.Start = function () {
            var n = window.location.href.lastIndexOf('/');
            $http.get(serverBaseUrl + '/api/Courses/Get?ID=' + window.location.href.substring(n + 1),option)
            .then(function (response) {
                $scope.course = response.data;
            })
        }
    }]);
    //Create
    app.controller("Courses_Create_Ctrl", ["$scope", "$http", function ($scope, $http) {
        $scope.Treverse = false;
        $scope.Sreverse = false;

        $scope.orderBySubject = function (type) {
            orderBy(type, "Subject");
        };
        $scope.orderByTeacher = function (type) {
            orderBy(type, "Teacher");
        };
        function orderBy(t, o) {
            if (o == "Teacher") {
                $scope.Treverse = ($scope.TheOrderByTeacher === t) ? !$scope.Treverse : false;
                $scope.TheOrderByTeacher = t;
            }
            else {
                $scope.Sreverse = ($scope.TheOrderBySubject === t) ? !$scope.Sreverse : false;
                $scope.TheOrderBySubject = t;
            }
        }

        $scope.Start = function () {
            var option = { headers: getHeaders() };
            $http.get(serverBaseUrl+'/api/Subjects/Get',option)
                .then(function (response) {
                    $scope.subjects = JSON.parse(JSON.stringify(response.data));
                    $scope.subject = $scope.subjects[0].ID;

                    $http.get(serverBaseUrl+'/api/UsersAPI/GetAvailableTeachers?subjectID=' + $scope.subject,option)
                        .then(function (response) {
                            $scope.teachers = JSON.parse(JSON.stringify(response.data));
                            orderBy("Name", "Subject");
                            orderBy("LastName", "Teacher");
                            $scope.teacher = $scope.teachers[0].Id;
                        });
                });
        }

        function uData() {
            var option = { headers: getHeaders() };
            $http.get(serverBaseUrl+'/api/UsersAPI/GetAvailableTeachers?subjectID=' + $scope.subject,option)
                .then(function (response) {
                    $scope.teachers = JSON.parse(JSON.stringify(response.data));
                });
        }

        $scope.update = function () {
            uData();
        }
        $scope.Create = function () {
            var courseData = {
                TeacherID: $scope.teacher,
                SubjectID: $scope.subject
            };
            $http({
                method: 'POST',
                url: serverBaseUrl + '/api/Courses/Create',
                data: courseData
            }).then(function (response) {
                window.location.href = "/Courses";
            }).then(function (err) {
                $scope.ErrorMessage = "Error " + err.status + " : " + err.data;
            })
        }
    }]);
    function getHeaders() {
            return { "Authorization": "Bearer " + sessionStorage.getItem("token"), 'Content-Type': 'application/json' };
    }
}());