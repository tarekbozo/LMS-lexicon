(function () {
    var app = angular.module("LMSApp", ['ui.router']);

    app.config(['$stateProvider', function ($stateProvider) {

        $stateProvider
            .state('home', {
                url: '/',
                controller: 'HomeController',
                controllerAs:'home',
                templateUrl: 'Views/Home.html',
            })

    }])

}());