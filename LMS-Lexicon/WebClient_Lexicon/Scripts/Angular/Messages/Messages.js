(function () {
    var app = angular.module("LMSApp");
    var serverBaseUrl = "http://localhost:51942";
    var option = { headers: getHeaders() };

    //index
    app.controller("Messages_Index_Ctrl", ["$scope", "$http", function ($scope, $http) {
        $scope.label = "Inbox";
        $scope.buttonFilter = {};
        $scope.InitialFilter = {
            PermantDelete : false,
            Trash : false
        };
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
        $scope.getSent=function(){
            $scope.buttonFilter = {};
            $scope.buttonFilter.Owned = 'true';
            $scope.InitialFilter.Trash = 'false';
            $scope.label = "Sent";

        }
        $scope.getFavorite = function () {
            $scope.buttonFilter = {};
            $scope.buttonFilter.Favorite = 'true';
            $scope.InitialFilter.Trash = 'false';
            $scope.label = "Favorite";

        };
        $scope.getImportant = function () {
            $scope.buttonFilter = {};
            $scope.buttonFilter.Important = 'true';
            $scope.InitialFilter.Trash = 'false';
            $scope.label = "Important";

        };
        $scope.getTrash = function () {
            $scope.buttonFilter = {};
            $scope.buttonFilter.Trash = 'true';
            $scope.InitialFilter.Trash = 'true';
            $scope.label = "Trash";
        };
        $scope.getInbox = function () {
            $scope.buttonFilter = {};
            $scope.InitialFilter.Trash = 'false';
            $scope.label = "Inbox";
        }
        $scope.refresh = function () {
            $http.get(serverBaseUrl + '/api/Messages/GetMessages', option)
                .then(function (response) {
                    $scope.messages = JSON.parse(JSON.stringify(response.data));
                });
        }
        $scope.setFavorite = function (id) {
            var serverBaseUrl = "http://localhost:51942";
            $http.get(serverBaseUrl + '/api/Messages/GetMessage?ID='+id, option)
                .then(function (response) {
                    var message = JSON.parse(JSON.stringify(response.data));
                    var setting = { ID: message.ID, Email: $scope.user.Email };
                    $http({
                        method: 'PUT',
                        url: serverBaseUrl + '/api/Messages/SetMailAsFavorite/',
                        data: JSON.stringify(setting),
                        headers: getHeaders()
                    }).then(function (response) {
                        $http.get(serverBaseUrl + '/api/Messages/GetMessages', option)
                            .then(function (response) {
                                $scope.messages = JSON.parse(JSON.stringify(response.data));
                            });
                    }).then(function (err) {
                    })
                });            
        }
        $scope.setTrash = function (id) {
            var serverBaseUrl = "http://localhost:51942";
            $http.get(serverBaseUrl + '/api/Messages/GetMessage?ID=' + id, option)
                .then(function (response) {
                    var message = JSON.parse(JSON.stringify(response.data));
                    var setting = { ID: message.ID, Email: $scope.user.Email };
                    $http({
                        method: 'PUT',
                        url: serverBaseUrl + '/api/Messages/SetMailAsTrash/',
                        data: JSON.stringify(setting),
                        headers: getHeaders()
                    }).then(function (response) {
                        $http.get(serverBaseUrl + '/api/Messages/GetMessages', option)
                            .then(function (response) {
                                $scope.messages = JSON.parse(JSON.stringify(response.data));
                            });
                    }).then(function (err) {
                    })
                });
        }
        $scope.setImportant = function (id) {
            var serverBaseUrl = "http://localhost:51942";
            $http.get(serverBaseUrl + '/api/Messages/GetMessage?ID=' + id, option)
                .then(function (response) {
                    var message = JSON.parse(JSON.stringify(response.data));
                    var setting = { ID: message.ID, Email: $scope.user.Email };
                    $http({
                        method: 'PUT',
                        url: serverBaseUrl + '/api/Messages/SetMailAsImportant/',
                        data: JSON.stringify(setting),
                        headers: getHeaders()
                    }).then(function (response) {
                        $http.get(serverBaseUrl + '/api/Messages/GetMessages', option)
                            .then(function (response) {
                                $scope.messages = JSON.parse(JSON.stringify(response.data));
                            });
                    }).then(function (err) {
                    })
                });
        }
        $scope.Delete = function (id) {
            var serverBaseUrl = "http://localhost:51942";
            $http.get(serverBaseUrl + '/api/Messages/GetMessage?ID=' + id, option)
                .then(function (response) {
                    var message = JSON.parse(JSON.stringify(response.data));
                    console.log(message);
                    $http({
                        method: 'DELETE',
                        url: serverBaseUrl + '/api/Messages/Delete/',
                        data: JSON.stringify(message),
                        headers: { "Authorization": "Bearer " + sessionStorage.getItem("token"), 'Content-Type': 'application/json' }

                    }).then(function (response) {
                        $http.get(serverBaseUrl + '/api/Messages/GetMessages', option)
                            .then(function (response) {
                                $scope.messages = JSON.parse(JSON.stringify(response.data));
                            });
                    }).then(function (error) {
                    })
                });
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