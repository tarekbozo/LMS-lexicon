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

        vm.registerUser = registerUser;
        vm.loginUser = loginUser;
        vm.getValues = getValues;
        vm.logOutUser = logOut;
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
                vm.bearerToken = data.access_token;
                
                sessionStorage.setItem("username", vm.userName);
                sessionStorage.setItem("isLoggedIn", vm.isLoggedIn);

            }, function (error, status) {
                vm.isLoggedIn = false;
                console.log(status);
            });
        }
        function logOut() {
            userAccountService.logOutCurrentUser().then(function(response) {
                vm.isLoggedIn = false;
                vm.userName = "";
                vm.bearerToken = null;
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