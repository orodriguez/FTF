using System;
using System.Linq;
using FTF.Core.Entities;

namespace FTF.Core.Notes
{
    public class UpdateHandler
    {
        private readonly IQueryable<Note> _notes;

        private readonly Action _saveChanges;

        public UpdateHandler(IQueryable<Note> notes, Action saveChanges)
        {
            _notes = notes;
            _saveChanges = saveChanges;
        }

        public void Update(int id, string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new Exception("Note can not be empty");

            var noteToUpdate = _notes.First(n => n.Id == id);

            noteToUpdate.Text = text;

            _saveChanges();
        }
    }
}