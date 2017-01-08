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

        [Given(@"I created the note number (.*) with text '(.*)'")]
        public void CreateNote(int id, string text)
        {
            Context.NextId = () => id;
            Catch(() => _createNote(text));
        }

        [Given(@"I created a note with text '(.*)'")]
        [When(@"I create a note with text '(.*)'")]
        public void CreateNote(string text) => Catch(() => _createNote(text));

        [Given(@"I deleted the note (.*)")]
        public void DeleteNote(int id) => _delete(id);

        [Given(@"I created the following notes:")]
        public void CreateNotes(Table table) => 
            table.CreateSet<Row>()
                .ToList()
                .ForEach(r => CreateNote(r.Text));

        private class Row
        {
            public string Text { get; set; }
        }
    }
}