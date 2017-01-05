using System;
using System.Linq;
using FTF.Core.Entities;

namespace FTF.Core.Notes
{
    public class DeleteHandler
    {
        private readonly IQueryable<Note> _notes;

        private readonly Action _saveChanges;
        private readonly IQueryable<Tag> _tags;

        public DeleteHandler(
            IQueryable<Note> notes, 
            Action saveChanges, IQueryable<Tag> tags)
        {
            _notes = notes;
            _saveChanges = saveChanges;
            _tags = tags;
        }

        public void Delete(int id)
        {
            var note = _notes.First(n => n.Id == id);

            var trashTag = _tags.FirstOrDefault(t => t.Name == "Trash") ?? new Tag { Name = "Trash" };

            note.Taggings.Add(new Tagging
            {
                Note = note,
                Tag = trashTag
            });

            _saveChanges();
        }
    }
}