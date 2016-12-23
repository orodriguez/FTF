using System;
using FTF.Core;
using FTF.Core.Notes;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using DbContext = FTF.Core.DbContext;

namespace FTF.Specs
{
    [Binding]
    public class Steps
    {
        private Note _note;

        private Exception _error;

        
        private Context _context;

        public Steps(Context context)
        {
            _context = context;
        }

        [Given(@"today is '(.*)'")]
        public void TodayIs(string date) => _context.GetCurrentDate = () => DateTime.Parse(date);

        [Given(@"I created a note \#(.*) with text '(.*)'")]
        public void CreateNote(int id, string text) =>
            new CreateNote(generateId: () => id, getCurrentDate: _context.GetCurrentDate, db: _context.Db).Create(id, text);

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

        [BeforeScenario()]
        public void BeforeScenario()
        {
            _context.Db = new DbContext("FTF.Tests");
            _context.Transaction = _context.Db.Database.BeginTransaction();
        }

        [AfterScenario()]
        public void AfterScenario()
        {
            _context.Transaction.Rollback();
            _context.Db.Dispose();
            _context = null;
        }
    }
}
