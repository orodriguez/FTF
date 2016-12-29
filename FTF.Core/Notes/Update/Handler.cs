using System;
using System.Linq;
using FTF.Core.Entities;

namespace FTF.Core.Notes.Update
{
    public class Handler
    {
        private readonly IQueryable<Note> _notes;

        private readonly Action _saveChanges;

        public Handler(IQueryable<Note> notes, Action saveChanges)
        {
            _notes = notes;
            _saveChanges = saveChanges;
        }

        public void Update(int id, string text)
        {
            var noteToUpdate = _notes.First(n => n.Id == id);

            noteToUpdate.Text = text;

            _saveChanges();
        }
    }
}