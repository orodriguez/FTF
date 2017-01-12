using System.Linq;
using FTF.Core.Attributes;
using FTF.Core.EntityFramework;

namespace FTF.Core.Notes
{
    [Concrete]
    public class UpdateHandler
    {
        private readonly DbContext _db;

        public UpdateHandler(DbContext db)
        {
            _db = db;
        }

        public void Update(int id, string text)
        {
            NoteValidator.Validate(text);

            var noteToUpdate = _db.Notes.First(n => n.Id == id);

            noteToUpdate.Text = text;

            _db.SaveChanges();
        }
    }
}