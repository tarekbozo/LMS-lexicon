(function () {
    var app = angular.module("LMSApp");
    var serverBaseUrl = "http://localhost:51942";
    var option = { headers: getHeaders() };

    //index
    app.controller("Messages_Index_Ctrl", ["$scope", "$http", function ($scope, $http) {
        $scope.reverse = false;
        $scope.myOrderBy = 'Name';
        $scope.index = 0;

        $scope.orderByMe = function (type) {
            $scope.reverse = ($scope.myOrderBy === type) ? !$scope.reverse : false;
            $scope.myOrderBy = type;
        };
        $scope.Start = function () {
            var _url = serverBaseUrl + "/api/UsersAPI/GetUserInfoFromCurrentUser?userName=" + sessionStorage.getItem("username");
            $http({
                method: 'GET',
                url: _url,
                headers: getHeaders(),
            }).then(function (response) {
                $scope.user = response.data;
            });
            $http.get(serverBaseUrl + '/api/Messages/GetMessages', option)
                .then(function (response) {
                    $scope.messages = JSON.parse(JSON.stringify(response.data));
                });
        };
        $scope.Details=function(index){
        }
        $scope.Favorite = function () {
            document.getElementById("favorite").style.color = "gold";
        }
    }]);
    //SendMail
    app.controller("Messages_Send_Ctrl", ["$scope", "$http", function ($scope, $http) {
        $scope.Send = function () {
            var message;
            var _url = serverBaseUrl + "/api/UsersAPI/GetUserInfoFromCurrentUser?userName=" + sessionStorage.getItem("username");
            $http({
                method: 'GET',
                url: _url,
                headers: getHeaders(),
            }).then(function (response) {
                $scope.user = response.data;
                message = {
                    Description: $scope.description,
                    MessageContent: $scope.messageContent,
                    EmailFrom: $scope.user.Email,
                    EmailTo: $scope.EmailTo
                };
                _url = serverBaseUrl + "/api/Messages/Send";
                $http({
                    method: 'POST',
                    url: _url,
                    data: JSON.stringify(message),
                    headers: getHeaders(),
                }).then(function (response) {
                    alert("Message is sent");
                })
            });
        };

    }]);

    function getHeaders() {
        return { "Authorization": "Bearer " + sessionStorage.getItem("token"), 'Content-Type': 'application/json' };
    }
}());