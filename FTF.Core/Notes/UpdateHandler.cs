using System.Linq;
using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.Delegates.Actions.Notes;
using FTF.Core.Entities;

namespace FTF.Core.Notes
{
    [Concrete]
    public class UpdateHandler
    {
        private readonly IQueryable<Note> _notes;

        private readonly SaveChanges _saveChanges;

        private readonly ValidateNote _validate;

        public UpdateHandler(
            IQueryable<Note> notes, 
            ValidateNote validate, 
            SaveChanges saveChanges)
        {
            _notes = notes;
            _validate = validate;
            _saveChanges = saveChanges;
        }

        [Role(typeof(Update))]
        public void Update(int id, string text)
        {
            _validate(text);

            var noteToUpdate = _notes.First(n => n.Id == id);

            noteToUpdate.Text = text;

            _saveChanges();
        }
    }
}