using System.Linq;
using FTF.Api.Actions.Notes;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace FTF.Specs.Steps
{
    [Binding]
    public class NoteSteps : Steps
    {
        private readonly Create _createNote;

        private readonly Delete _delete;

        public NoteSteps(Context context, Create createNote, Delete delete) : base(context)
        {
            _createNote = createNote;
            _delete = delete;
        }


        [Given(@"I deleted the note (.*)")]
        public void DeleteNote(int id) => _delete(id);


        private class Row
        {
            public string Text { get; set; }
        }
    }
}