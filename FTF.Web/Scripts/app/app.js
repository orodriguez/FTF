/// <reference path="../angular.js"/>

"use strict";

var ftfApp = angular.module("ftfApp", []);

ftfApp.controller("NotesListController", function($scope) {
    $scope.notes = [
        {
            "Id": 18,
            "Text": "This is test",
            "CreationDate": "2017-01-16T16:05:32.943",
            "UserName": "orodriguez",
            "Tags": []
        },
        {
            "Id": 17,
            "Text": "This is test",
            "CreationDate": "2017-01-16T14:25:25.847",
            "UserName": "orodriguez",
            "Tags": []
        },
        {
            "Id": 16,
            "Text": "This is test",
            "CreationDate": "2017-01-16T14:25:22.573",
            "UserName": "orodriguez",
            "Tags": []
        }
    ];
});