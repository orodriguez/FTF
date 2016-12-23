using System;
using FTF.Core;
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

        private Note _note;

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
