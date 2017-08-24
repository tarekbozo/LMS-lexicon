(function () {
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
        };
        var serverBaseUrl = "http://localhost:51942";

        return service;
        var accessToken = "";
        function registerUser(userData) {
            var accountUrl = serverBaseUrl + "/api/Account/Register";
            var deferred = $q.defer();
            $http({
                method: 'POST',
                url: accountUrl,
                data: userData,
            }).then(function (response) {
                console.log(response.data);
                deferred.resolve(response.data);
            }, function (err) {
                alert("Account Couldn't be created, make sure the password contains a special character, a number and is at minimum 6 letters.");
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
                sessionStorage.setItem("token",accessToken);
                // check the log screen to know currently back from the server when a user log in successfully.
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
                sessionStorage.setItem("username","");
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
        // we have to include the Bearer token with each call to the Web API controllers. 
        function getHeaders() {
            if (accessToken) {
                return { "Authorization": "Bearer " + accessToken };
            }
            else
            {
                accessToken = sessionStorage.getItem("token");
                return { "Authorization": "Bearer " + accessToken }
            }
        }
    }
})();