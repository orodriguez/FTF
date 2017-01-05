using System.Linq;
using FTF.Api.Actions.Notes;
using FTF.Core.Extensions.Queriable;
using FTF.Core.Notes;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace FTF.Specs.Steps
{
    [Binding]
    public class NoteSteps
    {
        private readonly Context _context;

        public NoteSteps(Context context)
        {
            _context = context;
        }

        [Given(@"I created the note number (.*) with text '(.*)'")]
        public void CreateNote(int id, string text)
        {
            Create create = new CreateHandler(
                generateId: () => id,
                getCurrentDate: _context.GetCurrentDate,
                saveNote: note => _context.Db.Notes.Add(note),
                saveChanges: () => _context.Db.SaveChanges(),
                tags: _context.Db.Tags,
                getCurrentUser: () => _context.CurrentUser,
                validate: NoteValidator.Validate
            ).Create;

            create(text);
        }

        [Given(@"I created a note with text '(.*)'")]
        [When(@"I create a note with text '(.*)'")]
        public void CreateNote(string text)
        {
            _context.StoreException(() => {
                new CreateHandler(
                    generateId: () => _context.Db.Notes.NextId(),
                    getCurrentDate: _context.GetCurrentDate,
                    saveNote: note => _context.Db.Notes.Add(note),
                    saveChanges: () => _context.Db.SaveChanges(),
                    tags: _context.Db.Tags,
                    getCurrentUser: () => _context.CurrentUser,
                    validate: NoteValidator.Validate
                ).Create(text);
            });
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