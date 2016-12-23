using System;
using System.Data.Entity;
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
        private Note _note;

        private Func<DateTime> _getCurrentDate;

        private Exception _error;

        private FtfDbContext _db;

        private DbContextTransaction _transation;

        [Given(@"today is '(.*)'")]
        public void TodayIs(string date) => _getCurrentDate = () => DateTime.Parse(date);

        [Given(@"I created a note \#(.*) with text '(.*)'")]
        public void CreateNote(int id, string text) =>
            new CreateNote(generateId: () => id, getCurrentDate: _getCurrentDate, db: _db).Create(id, text);

        [When(@"I retrieve the note \#(.*)")]
        public void RetrieveNote(int id)
        {
            try
            {
                _note = new Queries(_db.Notes).Retrieve(id);
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

        [BeforeScenario()]
        public void BeforeScenario()
        {
            _db = new FtfDbContext("FTF.Tests");
            _transation = _db.Database.BeginTransaction();
        }

        [AfterScenario()]
        public void AfterScenario()
        {
            _transation.Rollback();
            _db.Dispose();
            _db = null;
        }
    }
}
