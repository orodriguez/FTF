using FTF.Api.Actions.Notes;
using FTF.Api.Responses;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace FTF.Specs.Steps
{
    [Binding]
    public class RetrieveNoteSteps : Steps
    {
        private INote _response;

        private readonly Retrieve _retrieveNote;

        public RetrieveNoteSteps(Context context, Retrieve retrieveNote) : base(context)
        {
            _retrieveNote = retrieveNote;
        }

        [When(@"I retrieve the note number (.*)")]
        public void RetrieveNote(int id) => _response = _retrieveNote(id);

        [Then(@"the note should match:")]
        public void NoteShouldMatch(Table table) => table.CompareToInstance(_response);

        [Then(@"it should show the error '(.*)'")]
        public void ShouldShowError(string message)
        {
            Assert.NotNull(Exception, "No error was produced");
            Assert.AreEqual(message, Exception.Message);
        }

        [Then(@"the note should contain the tags:")]
        public void NoteShouldContainTags(Table table) => table.CompareToSet(_response.Tags);
    }
}
