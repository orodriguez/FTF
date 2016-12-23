using System;
using System.Collections.Generic;
using System.Linq;
using FTF.Core.Extensions;
using FTF.Core.Extensions.Queriable;
using FTF.Core.Queries;

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
                Tags = MakeTags(text.ParseTagNames()).ToList()
            });

            _saveChanges();
        }

        private IEnumerable<Tag> MakeTags(string[] tagNames) => 
            _tags
                .ByName(tagNames)
                .ToArray()
                .Concat(MakeNewTags(tagNames));

        private IEnumerable<Tag> MakeNewTags(string[] tagNames) => 
            tagNames
                .Except(_tags.ByName(tagNames).Names())
                .Select(tagName => new Tag { Name = tagName })
                .ToArray();
    }
}