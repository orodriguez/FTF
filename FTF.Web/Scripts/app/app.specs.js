/// <reference path="../_references.js"/>

"use strict";

describe('NotesListController', function () {

    beforeEach(module('ftfApp'));

    it('should create a `notes` model with 3 notes', inject(function ($controller) {
        var scope = {};
        var ctrl = $controller('NotesListController', { $scope: scope });

        expect(scope.notes.length).toBe(3);
    }));


    it('should be true', function() { expect(true).toBe(true)});
});