using System.Linq;
using FTF.Api.Actions.Notes;
using FTF.Core.Extensions.Queriable;
using FTF.Core.Notes;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace FTF.Specs.Steps
{
    [Binding]
    public class NoteSteps : Steps
    {
        public NoteSteps(Context context) : base(context)
        {
        }

        [Given(@"I created the note number (.*) with text '(.*)'")]
        public void CreateNote(int id, string text)
        {
            Create create = new CreateHandler(
                generateId: () => id,
                getCurrentDate: () => Context.GetCurrentDate(),
                saveNote: note => Context.Db.Notes.Add(note),
                saveChanges: () => Context.Db.SaveChanges(),
                tags: Context.Db.Tags,
                getCurrentUser: () => Context.CurrentUser,
                validate: NoteValidator.Validate
            ).Create;

            create(text);
        }

        [Given(@"I created a note with text '(.*)'")]
        [When(@"I create a note with text '(.*)'")]
        public void CreateNote(string text) => Exec<Create>(f => f(text));

        [Given(@"I deleted the note (.*)")]
        public void DeleteNote(int id)
        {
            DeleteNoted delete = new DeleteHandler(
                notes: Context.Db.Notes, 
                tags: Context.Db.Tags,
                saveChanges:() => Context.Db.SaveChanges()
            ).Delete;

            delete(id);
        }

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