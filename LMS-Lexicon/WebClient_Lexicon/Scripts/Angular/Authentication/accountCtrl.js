(function () {
    'use strict';
    var controllerId = 'AccountCtrl';
    angular.module('LMSApp').controller(controllerId,
        ['userAccountService', accountCtrl]);
    function accountCtrl(userAccountService,$window) {
        // Using 'Controller As' syntax, so we assign this to the vm variable (for viewmodel).
        var vm = this;
        // Bindable properties and functions are placed on vm.
        vm.title = 'AccountCtrl';
        vm.isRegistered = false;

        vm.isLoggedIn = sessionStorage.getItem("isLoggedIn");
        vm.userName = sessionStorage.getItem("username");

        vm.registerUserData = {
            email: "",
            password: "",
            confirmPassword: "",
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

        function getRoleNames() {
            userAccountService.getRoleNames().then(function (r) {
                vm.roles2 = r;
                vm.selectedRole = r[0];
            })
        }
        function registerUser() {
            userAccountService.registerUser(vm.registerUserData).then(function (data) {
                vm.isRegistered = true;
            }, function (error, status) {
                vm.isRegistered = false;
                console.log(status);
            });
        }
        function loginUser() {
            userAccountService.loginUser(vm.loginUserData).then(function (data) {
                vm.isLoggedIn = true;
                vm.userName = data.userName;

                sessionStorage.setItem("username", vm.userName);
                sessionStorage.setItem("isLoggedIn", vm.isLoggedIn);

                window.location.href="/";
            }, function (error, status) {
                vm.isLoggedIn = false;
                console.log(status);
            });
        }
        function logOut() {
            userAccountService.logOutCurrentUser().then(function(response) {
                vm.isLoggedIn = false;
                vm.userName = "";
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