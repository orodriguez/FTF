using System;
using System.Linq;
using FTF.Core.Entities;

namespace FTF.Core.Notes
{
    public class UpdateHandler
    {
        private readonly IQueryable<Note> _notes;

        private readonly Action _saveChanges;

        private readonly Action<string> _validate;

        public UpdateHandler(
            IQueryable<Note> notes, 
            Action<string> validate, 
            Action saveChanges)
        {
            _notes = notes;
            _validate = validate;
            _saveChanges = saveChanges;
        }

        public void Update(int id, string text)
        {
            _validate(text);

            var noteToUpdate = _notes.First(n => n.Id == id);

            noteToUpdate.Text = text;

            _saveChanges();
        }
    }
}