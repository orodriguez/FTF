﻿using System;
using FTF.Core;
using FTF.Core.Notes;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace FTF.Specs
{
    [Binding]
    public class Steps
    {
        private readonly Context _context;

        private Note _note;

        private Exception _error;

        public Steps(Context context)
        {
            _context = context;
        }

        [Given(@"today is '(.*)'")]
        public void TodayIs(string date) => _context.GetCurrentDate = () => DateTime.Parse(date);

        [Given(@"I created a note \#(.*) with text '(.*)'")]
        public void CreateNote(int id, string text) =>
            new CreateNote(
                generateId: () => id, 
                getCurrentDate: _context.GetCurrentDate, 
                saveNote: note => _context.Db.Notes.Add(note), 
                saveChanges: () => _context.Db.SaveChanges()
            ).Create(id, text);

        [When(@"I retrieve the note \#(.*)")]
        public void RetrieveNote(int id)
        {
            try
            {
                _note = new Queries(_context.Db.Notes).Retrieve(id);
            }
            catch (Exception e)
            {
                _error = e;
            }
        }

        [Then(@"the note should match:")]
        public void NoteShouldMatch(Table table) => table.CompareToInstance(_note);

        [Then(@"it should show the error '(.*)'")]
        public void ShouldShowError(string message)
        {
            Assert.NotNull(_error, "No error was produced");
            Assert.AreEqual(message, _error.Message);
        }
    }
}
