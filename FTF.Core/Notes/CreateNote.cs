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

        private readonly IQueryable<Tag> _tags;

        public CreateNote(
            Func<int> generateId, 
            Func<DateTime> getCurrentDate, 
            Action<Note> saveNote, 
            Action saveChanges, 
            IQueryable<Tag> tags)
        {
            _generateId = generateId;
            _getCurrentDate = getCurrentDate;
            _saveNote = saveNote;
            _saveChanges = saveChanges;
            _tags = tags;
        }

        public void Create(string text)
        {
            _saveNote(new Note
            {
                Id = _generateId(),
                Text = text,
                CreationDate = _getCurrentDate(),
                Tags = ParseTags(text).ToArray()
            });

            _saveChanges();
        }

        private IEnumerable<Tag> ParseTags(string text)
        {
            var tagNames = text
                .ParseTagNames()
                .Distinct()
                .ToArray();

            var existingTags = FindExistingTags(tagNames);

            var newTags = tagNames.Where(tagName => existingTags.All(t => t.Name != tagName))
                .Select(tagName => new Tag { Name = tagName })
                .ToArray();

            return existingTags.Concat(newTags);
        }

        private List<Tag> FindExistingTags(IEnumerable<string> tagNames)
        {
            return _tags
                .Where(t => tagNames.Contains(t.Name))
                .ToList();
        }
    }
}