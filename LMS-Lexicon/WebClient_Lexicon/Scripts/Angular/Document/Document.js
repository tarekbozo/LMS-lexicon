(function () {
    var app = angular.module("LMSApp");
    app.controller("DocumentAPIController", ["$scope", "$http", function ($scope, $http) {
        var serverBaseUrl = "http://localhost:51942";

        self.upload = function () {
            if (document.getElementById("upload").value != "") {
                var file = document.getElementById("upload").files[0];
                var filePath = "....";
                if (window.FormData !== undefined) {
                    var data = new FormData();
                    data.append("file", file);
                    var encodedString = Base64.encode(filePath);

                    $.ajax({
                        type: "POST",
                        url: "/API/document/upload/" + file.name + "/" + encodedString ,
                        contentType: false,
                        processData: false,
                        data: data,
                        success: function (result) {
                            console.log(result);
                        },
                        error: function (xhr, status, p3, p4) {
                            var err = "Error " + " " + status + " " + p3 + " " + p4;
                            if (xhr.responseText && xhr.responseText[0] == "{")
                                err = JSON.parse(xhr.responseText).Message;
                            console.log(err);
                        }
                    }
      
                    )};
            }
        }
    }])})