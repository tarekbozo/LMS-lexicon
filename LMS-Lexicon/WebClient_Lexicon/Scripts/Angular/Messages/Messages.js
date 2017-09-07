(function () {
    var app = angular.module("LMSApp");
    var serverBaseUrl = "http://localhost:51942";
    var option = { headers: getHeaders() };

    //index
    app.controller("Messages_Index_Ctrl", ["$scope", "$http", function ($scope, $http) {
        $scope.reverse = false;
        $scope.myOrderBy = 'Name';

        $scope.orderByMe = function (type) {
            $scope.reverse = ($scope.myOrderBy === type) ? !$scope.reverse : false;
            $scope.myOrderBy = type;
        };
        $scope.Start = function () {
            $http.get(serverBaseUrl + '/api/Messages/GetMessages', option)
                .then(function (response) {
                    $scope.messages = JSON.parse(JSON.stringify(response.data));
                });
        };

    }]);
    //SendMail
    app.controller("Messages_Send_Ctrl", ["$scope", "$http", function ($scope, $http) {
        $scope.Send = function () {
            var m = {
                MessageContent: $scope.messageContent,
                EmailFrom: $scope.user.Email,
                EmailTo: $scope.recievingEmail
            };
        };

    }]);
    function getHeaders() {
        return { "Authorization": "Bearer " + sessionStorage.getItem("token"), 'Content-Type': 'application/json' };
    }
}());