using System;
using System.Collections.Generic;
using System.Linq;
using FTF.Core.Extensions;

namespace FTF.Core.Notes
{
    public class CreateNote
    {
        private readonly Func<int> _generateId;

        private readonly Func<DateTime> _getCurrentDate;

        private readonly Action<Note> _saveNote;

        private readonly Action _saveChanges;

        public CreateNote(Func<int> generateId, Func<DateTime> getCurrentDate, Action<Note> saveNote, Action saveChanges)
        {
            _generateId = generateId;
            _getCurrentDate = getCurrentDate;
            _saveNote = saveNote;
            _saveChanges = saveChanges;
        }

        public void Create(string text)
        {
            _saveNote(new Note
            {
                Id = _generateId(),
                Text = text,
                CreationDate = _getCurrentDate(),
                Tags = ParseTags(text)
            });

            _saveChanges();
        }

        private ICollection<Tag> ParseTags(string text) => 
            text
                .ParseTagNames()
                .Distinct()
                .Select(_ => new Tag { Name = _ })
                .ToList();
    }
}