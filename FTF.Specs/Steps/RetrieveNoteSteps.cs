﻿using System;
using FTF.Api.Actions.Notes;
using FTF.Api.Responses;
using FTF.Core.Notes;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace FTF.Specs.Steps
{
    [Binding]
    public class RetrieveNoteSteps
    {
        private readonly Context _context;

        private INote _response;

        private Exception _error;

        public RetrieveNoteSteps(Context context)
        {
            _context = context;
        }

        [When(@"I retrieve the note number (.*)")]
        public void RetrieveNote(int id)
        {
            try
            {
                Retrieve retrieve = new Queries(_context.Db.Notes).Retrieve;

                _response = retrieve(id);
            }
            catch (Exception e)
            {
                _error = e;
            }
        }

        [Then(@"the note should match:")]
        public void NoteShouldMatch(Table table) => table.CompareToInstance(_response);

        [Then(@"it should show the error '(.*)'")]
        public void ShouldShowError(string message)
        {
            Assert.NotNull(_error, "No error was produced");
            Assert.AreEqual(message, _error.Message);
        }

        [Then(@"the note should contain the tags:")]
        public void NoteShouldContainTags(Table table) => table.CompareToSet(_response.Tags);
    }
}
