using FTF.Api.Actions.Notes;
using TechTalk.SpecFlow;

namespace FTF.Specs.Steps
{
    [Binding]
    public class UpdateNoteSteps : Steps
    {
        private readonly Update _updateNote;

        public UpdateNoteSteps(Context context, Update updateNote) : base(context)
        {
            _updateNote = updateNote;
        }

        [Given(@"I updated the note number (.*) with text '(.*)'")]
        [When(@"I updated the note number (.*) with text '(.*)'")]
        public void UpdateNote(int id, string text) => Catch(() => _updateNote(id, text));
    }
}
