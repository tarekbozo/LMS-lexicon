(function () {
    'use strict';
    var controllerId = 'AccountCtrl';
    angular.module('LMSApp').controller(controllerId,
        ['userAccountService', '$routeParams', accountCtrl]);
    function accountCtrl(userAccountService, $window, $routeParams) {
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;
        // Bindable properties and functions are placed on vm.
        vm.title = 'AccountCtrl';
        vm.isRegistered = false;
        if (sessionStorage.getItem("isLoggedIn") != undefined) {
            vm.isLoggedIn = sessionStorage.getItem("isLoggedIn");
        }
        else {
            vm.isLoggedIn = false;
        }
        vm.userName = sessionStorage.getItem("username");
        
        vm.Role = sessionStorage.getItem("role");

        vm.registerUserData = {
            UserName: "",
            FirstName: "",
            LastName: "",
            BirthDate: "",
            RoleName: "",
            Email: ""
        };

        vm.loginUserData = {
            userName: "",
            password: "",
        };

        vm.roles = getRoleNames;
        vm.registerUser = registerUser;
        vm.loginUser = loginUser;
        vm.getValues = getValues;
        vm.logOutUser = logOut;
        vm.userInfo = userInfo;
        vm.deleteUser = deleteUser;

        vm.initialUser = function () {
            if (vm.isLoggedIn) {
                userInfo(vm.userName);
            }
        }
        vm.deleteUserInfo = function () {
            userInfo(vm.DeleteId);
        }
        vm.getRoute = function () {
            //vm.id = $routeParams.userid - routeParams returns undefined don't know why;
            //new solution for getting parameters
            var n = window.location.href.lastIndexOf('/');
            vm.DeleteId = window.location.href.substring(n + 1);
        }
        function userInfo(name) {
            if (name != "" && name && name != "null") {
                userAccountService.userInfo(name).then(function (r) {
                    vm.user = r;
                    if (sessionStorage.getItem("username") == r.UserName) {
                        sessionStorage.setItem("role", r.Role);
                    }
                })
            }
        }
        function getRoleNames() {
            userAccountService.getRoleNames().then(function (r) {
                vm.roles2 = r;
                vm.registerUserData.RoleName = r[1].Name;
            })
        }
        function deleteUser(answer) {
            if (answer == "yes") {
                userAccountService.deleteUser(vm.DeleteId).then(function (data) {
                    vm.DeleteId = "";
                    window.location.href = "/Users";
                });
            }
        }
        function registerUser() {
            if (isNaN(Date.parse(vm.registerUserData.BirthDate)) == false) {
                userAccountService.registerUser(vm.registerUserData).then(function (data) {
                    vm.isRegistered = true;
                    window.location.href = "/Users";
                }, function (error, status) {
                    vm.isRegistered = false;
                    console.log(status);
                    if (status < 500 && status > 300) {
                        vm.ErrorMessage = "Error: " + status + " - " + "Bad-Request please check the input fields";
                    }
                });
            }
            else {
                vm.ErrorMessage = "Date is invalid, make sure it have the format yyyy-mm-dd";
            }
        }
        function loginUser() {
            userAccountService.loginUser(vm.loginUserData).then(function (data) {
                vm.isLoggedIn = true;
                vm.userName = data.userName;
                sessionStorage.setItem("username", vm.userName);
                sessionStorage.setItem("isLoggedIn", vm.isLoggedIn);
                window.location.href = "/";
            }, function (error, status) {
                vm.isLoggedIn = false;
                console.log(status);
            });
        }
        function logOut() {
            userAccountService.logOutCurrentUser().then(function (response) {
                vm.isLoggedIn = false;
                vm.userName = "";
                sessionStorage.setItem("username", "");
                sessionStorage.setItem("token", "");
                sessionStorage.setItem("isLoggedIn", false);
                sessionStorage.setItem("role", null);
                window.location.href = "/";
            }, function (err) {
                alert("Something is not right");
            });
        }
        function getValues() {
            userAccountService.getValues().then(function (data) {
                vm.values = data;
                console.log('back... with success');
            });
        }
    }
})();