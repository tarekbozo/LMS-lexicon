(function () {
    'use strict';

    // Module name is handy for logging
    var id = 'LMSApp';

    // Create the module and define its dependencies.
    var app = angular.module('LMSApp', ['ngRoute']);

    app.config(['$httpProvider', function ($httpProvider) {
        // Use x-www-form-urlencoded Content-Type
        $httpProvider.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';

        // Override $http service's default transformRequest
        $httpProvider.defaults.transformRequest = [function (data) {
            /**
             * The workhorse; converts an object to x-www-form-urlencoded serialization.
             * @param {Object} obj
             * @return {String}
             */
            var param = function (obj) {
                var query = '';
                var name, value, fullSubName, subName, subValue, innerObj, i;

                for (name in obj) {
                    value = obj[name];

                    if (value instanceof Array) {
                        for (i = 0; i < value.length; ++i) {
                            subValue = value[i];
                            fullSubName = name + '[' + i + ']';
                            innerObj = {};
                            innerObj[fullSubName] = subValue;
                            query += param(innerObj) + '&';
                        }
                    }
                    else if (value instanceof Object) {
                        for (subName in value) {
                            subValue = value[subName];
                            fullSubName = name + '[' + subName + ']';
                            innerObj = {};
                            innerObj[fullSubName] = subValue;
                            query += param(innerObj) + '&';
                        }
                    }
                    else if (value !== undefined && value !== null) {
                        query += encodeURIComponent(name) + '=' + encodeURIComponent(value) + '&';
                    }
                }

                return query.length ? query.substr(0, query.length - 1) : query;
            };

            return angular.isObject(data) && String(data) !== '[object File]' ? param(data) : data;
        }];

    }]);

    app.config(
        function ($locationProvider, $routeProvider) {
        $locationProvider.html5Mode(true);
        $routeProvider.
        when('/', {
            templateUrl: 'Views/Home/Home.html',
            controller: 'HomeController'
        }).
        when('/Test', {
            template: '<h3>testing</h3>',
        }).
        when('/Account', {
            templateUrl: 'Views/Account/Account.html',
            controller: 'AccountCtrl'
        }).
        when('/Account/Delete/:userid', {
            templateUrl: 'Views/Account/Delete.html',
            controller: 'AccountCtrl'
        }).

        when('/Login', {
            templateUrl: 'Views/Account/Login.html',
            controller: 'AccountCtrl'
        }).
        when('/Account/Register', {
            templateUrl: 'Views/Account/Register.html',
            controller: 'AccountCtrl'
        }).
        when('/Users', {
            templateUrl: 'Views/User/Index.html',
            controller: 'UsersCtrl'
        }).
        when('/Subjects', {
            templateUrl: 'Views/Subjects/Index.html',
            controller: 'Subjects_IndexController'
        }).
        when('/Subjects/Create', {
            templateUrl: 'Views/Subjects/Create.html',
            controller: 'Subjects_CreateDeleteEditController'
        }).
        when('/Subjects/Edit/:subjectid', {
            templateUrl: 'Views/Subjects/Edit.html',
            controller: 'Subjects_CreateDeleteEditController'
        }).
        when('/Subjects/Details/:subjectid', {
            templateUrl: 'Views/Subjects/Details.html',
            controller: 'Subjects_CreateDeleteEditController'
        }).
        when('/Subjects/Delete/:subjectid', {
            templateUrl: 'Views/Subjects/Delete.html',
            controller: 'Subjects_CreateDeleteEditController'
        }).
        when('/Courses/Delete/:courseid', {
            templateUrl: 'Views/Courses/Delete.html',
            controller: 'Courses_Delete_Ctrl'
        }).
        when('/Courses/Details/:courseid', {
            templateUrl: 'Views/Courses/Details.html',
            controller: 'Courses_Details_Ctrl'
        }).
        when('/Courses', {
            templateUrl: 'Views/Courses/Index.html',
            controller: 'Courses_Index_Ctrl'
        }).
        when('/Courses/Edit/:courseid', {
            templateUrl: 'Views/Courses/Edit.html',
            controller: 'Courses_Edit_Ctrl'
        }).
        when('/Courses/Create', {
            templateUrl: 'Views/Courses/Create.html',
            controller: 'Courses_Create_Ctrl'
        }).
        when('/Messages', {
            templateUrl: 'Views/Messages/Index.html',
            controller: 'Messages_Index_Ctrl'
        }).
        when('/Messages/Send', {
            templateUrl: 'Views/Messages/Send.html',
            controller: 'Messages_Send_Ctrl'
        }).
        when('/Messages/Details/:mID', {
            templateUrl: 'Views/Messages/Details.html',
            controller: 'Messages_Details_Ctrl'
        }).
        otherwise({
            redirectTo: '/'
        });
  });
    // Execute bootstrapping code and any dependencies.
    app.run(['$q', '$rootScope',
        function ($q, $rootScope) {

        }]);
})();