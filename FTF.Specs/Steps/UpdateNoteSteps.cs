using FTF.Api.Actions.Notes;
using TechTalk.SpecFlow;

namespace FTF.Specs.Steps
{
    [Binding]
    public class UpdateNoteSteps
    {
        private readonly Update _updateNote;

        public UpdateNoteSteps(Context context, Update updateNote)
        {
            _updateNote = updateNote;
        }

        [Given(@"I updated the note number (.*) with text '(.*)'")]
        [When(@"I updated the note number (.*) with text '(.*)'")]
        public void UpdateNote(int id, string text) => _updateNote(id, text);
    }
}
