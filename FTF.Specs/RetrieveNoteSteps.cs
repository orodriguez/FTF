using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using FTF.Core;
using FTF.Core.Extensions;
using FTF.Core.Notes;
using FTF.Storage.EntityFramework;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace FTF.Specs
{
    [Binding]
    public class RetrieveNoteSteps
    {
        private readonly Context _context;

        private Note _note;

        private Exception _error;

        public RetrieveNoteSteps(Context context)
        {
            _context = context;
        }

        [Given(@"today is '(.*)'")]
        public void TodayIs(string date) => _context.GetCurrentDate = () => DateTime.Parse(date);

        [Given(@"I created the note number (.*) with text '(.*)'")]
        public void CreateNote(int id, string text) =>
            new CreateNote(
                generateId: () => id, 
                getCurrentDate: _context.GetCurrentDate, 
                saveNote: note => _context.Db.Notes.Add(note), 
                saveChanges: () => _context.Db.SaveChanges()
            ).Create(text);

        [Given(@"I created a note with text '(.*)'")]
        public void CreateNote(string text)
        {
            new CreateNote(
                generateId: () => _context.Db.Notes.NextId(),
                getCurrentDate: _context.GetCurrentDate,
                saveNote: note => _context.Db.Notes.Add(note),
                saveChanges: () => _context.Db.SaveChanges()
            ).Create(text);
        }

        [When(@"I retrieve the note number (.*)")]
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

        [Then(@"the note should contain the tags:")]
        public void NoteShouldContainTags(Table table) => table.CompareToSet(_note.Tags);
    }
}
