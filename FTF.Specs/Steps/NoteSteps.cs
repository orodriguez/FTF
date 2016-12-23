using FTF.Core.Extensions.Queriable;
using FTF.Core.Notes;
using TechTalk.SpecFlow;

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
        public void CreateNote(int id, string text) =>
            new CreateNote(
                generateId: () => id,
                getCurrentDate: _context.GetCurrentDate,
                saveNote: note => _context.Db.Notes.Add(note),
                saveChanges: () => _context.Db.SaveChanges(),
                tags: _context.Db.Tags
                ).Create(text);

        [Given(@"I created a note with text '(.*)'")]
        public void CreateNote(string text)
        {
            new CreateNote(
                generateId: () => _context.Db.Notes.NextId(),
                getCurrentDate: _context.GetCurrentDate,
                saveNote: note => _context.Db.Notes.Add(note),
                saveChanges: () => _context.Db.SaveChanges(),
                tags: _context.Db.Tags
                ).Create(text);
        }
    }
}