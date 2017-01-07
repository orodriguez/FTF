﻿using System.Linq;
using FTF.Core.Delegates;
using FTF.Core.Entities;

namespace FTF.Core.Notes
{
    public class DeleteHandler
    {
        private readonly IQueryable<Note> _notes;

        private readonly SaveChanges _saveChanges;

        private readonly IQueryable<Tag> _tags;

        public DeleteHandler(
            IQueryable<Note> notes, 
            SaveChanges saveChanges, IQueryable<Tag> tags)
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