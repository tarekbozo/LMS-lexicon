﻿(function () {
    'use strict';
    var serviceId = 'userAccountService';
    angular.module('LMSApp').factory(serviceId, ['$http', '$q', userAccountService]);
    function userAccountService($http, $q) {
        // Define the functions and properties to reveal.
        var service = {
            registerUser: registerUser,
            loginUser: loginUser,
            getValues: getValues,
            logOutCurrentUser: logOutCurrentUser,
            getRoleNames: getRoleNames,
            userInfo: userInfo,
            deleteUser: deleteUser,
        };
        var serverBaseUrl = "http://localhost:51942";

        return service;
        var accessToken = "";

        function userInfo(name) {
            var _url = serverBaseUrl + "/api/UsersAPI/GetUserInfoFromCurrentUser?userName=" + name;
            var deferred = $q.defer();
            $http({
                method: 'GET',
                url: _url,
                headers: getHeaders(),
            }).then(function (response) {
                console.log(response.data);
                deferred.resolve(response.data);
            })
            return deferred.promise;
        }
        function getRoleNames() {
            var _url = serverBaseUrl + "/api/UsersAPI/GetRoles";
            var deferred = $q.defer();
            $http({
                method: 'GET',
                url: _url,
                headers: getHeaders(),
            }).then(function (response) {
                console.log(response.data);
                deferred.resolve(response.data);
            }, function (err) {

                alert("something wet wrong.");
            })
            return deferred.promise;
        }

        function registerUser(userData) {
            var accountUrl = serverBaseUrl + "/api/Account/Register/";
            var deferred = $q.defer();
            var config = { headers: getHeaders() };
            $http.post(accountUrl, JSON.stringify(userData), config).then(function (response) {
                console.log(response.data);
                deferred.resolve(response.data);
            }, function (err) {
                alert(err.status);
            })
            return deferred.promise;
        }

        function deleteUser(userID) {
            var url = serverBaseUrl + "/api/Account/DeleteUser/";
            var deferred = $q.defer();
            var config = { headers: getHeaders() };
            $http({
                method: 'DELETE',
                url: url,
                data: JSON.stringify(userID),
                headers: getHeaders()
            }).then(function (response) {
                deferred.resolve(response.data);
            }, function (err) {
                alert("Couldn't Delete User");
            })
            return deferred.promise;
        }

        function loginUser(userData) {
            var tokenUrl = serverBaseUrl + "/Token";
            if (!userData.grant_type) {
                userData.grant_type = "password";
            }
            var deferred = $q.defer();
            $http({
                method: 'POST',
                url: tokenUrl,
                data: userData,
            }).then(function (response) {
                // save the access_token as this is required for each API call. 
                accessToken = response.data.access_token;
                sessionStorage.setItem("token", accessToken);
                console.log(response.data);
                deferred.resolve(response.data);
            }, function (err) {
                alert("The Username/Email or password is incorrect, please try again.");
            })
            return deferred.promise;
        }
        function logOutCurrentUser() {
            var deferred = $q.defer();
            var url = serverBaseUrl + "/api/Account/Logout";
            $http({
                method: 'POST',
                url: url,
                headers: getHeaders(),
            }).then(function (response) {
                accessToken = "";
                sessionStorage.setItem("token", "");
                sessionStorage.setItem("username", "");
                return deferred.resolve(response.data);
            }, function (err) {
                alert("Couldn't log out");
            });
            return deferred.promise;
        }
        function getValues() {
            var url = serverBaseUrl + "/api/home/get";
            var deferred = $q.defer();
            $http({
                method: 'GET',
                url: url,
                headers: getHeaders(),
            }).then(function (response) {
                console.log(response.data);
                deferred.resolve(response.data);
            })
            return deferred.promise;
        }
        function getHeaders() {
            if (accessToken) {
                return { "Authorization": "Bearer " + accessToken, 'Content-Type': 'application/json' };
            }
            else {
                accessToken = sessionStorage.getItem("token");
                return { "Authorization": "Bearer " + accessToken, 'Content-Type': 'application/json' }
            }
        }
    }
})();