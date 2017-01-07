using FTF.Api.Actions.Notes;
using FTF.Core.Notes;
using TechTalk.SpecFlow;

namespace FTF.Specs.Steps
{
    [Binding]
    public class UpdateNoteSteps : Steps
    {
        public UpdateNoteSteps(Context context) : base(context)
        {
        }

        [Given(@"I updated the note number (.*) with text '(.*)'")]
        [When(@"I updated the note number (.*) with text '(.*)'")]
        public void UpdateNote(int id, string text) => Exec<Update>(f => f(id, text));
    }
}
