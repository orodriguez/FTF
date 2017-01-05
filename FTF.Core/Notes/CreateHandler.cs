using System;
using System.Collections.Generic;
using System.Linq;
using FTF.Core.Entities;
using FTF.Core.Extensions;
using FTF.Core.Extensions.Queriable;
using FTF.Core.Queries;

namespace FTF.Core.Notes
{
    public class CreateHandler
    {
        private readonly Func<int> _generateId;

        private readonly Func<DateTime> _getCurrentDate;

        private readonly Action<Note> _saveNote;

        private readonly Action _saveChanges;

        private readonly IQueryable<Tag> _tags;

        private readonly Func<User> _getCurrentUser;

        private readonly Action<string> _validate;

        public CreateHandler(
            Func<int> generateId, 
            Func<DateTime> getCurrentDate, 
            Action<Note> saveNote, 
            Action saveChanges, 
            IQueryable<Tag> tags, 
            Func<User> getCurrentUser, 
            Action<string> validate)
        {
            _generateId = generateId;
            _getCurrentDate = getCurrentDate;
            _saveNote = saveNote;
            _saveChanges = saveChanges;
            _tags = tags;
            _getCurrentUser = getCurrentUser;
            _validate = validate;
        }

        public void Create(string text)
        {
            _validate(text);

            var note = new Note
            {
                Id = _generateId(),
                Text = text,
                CreationDate = _getCurrentDate(),
                User = _getCurrentUser()
            };

            note.Taggings = MakeTaggings(note, text.ParseTagNames()).ToList();

            _saveNote(note);

            _saveChanges();
        }

        private IEnumerable<Tagging> MakeTaggings(Note note, string[] tagNames) => 
            _tags
                .WhereNameContains(tagNames)
                .ToArray()
                .Select(tag => new Tagging { Note = note, Tag = tag })
                .Concat(MakeNewTaggings(note, tagNames));

        private IEnumerable<Tagging> MakeNewTaggings(Note note, string[] tagNames) => 
            tagNames
                .Except(_tags.WhereNameContains(tagNames).Names())
                .Select(tagName => new Tagging
                {
                    Note = note,
                    Tag = new Tag
                    {
                        Name = tagName,
                        User = _getCurrentUser()
                    }
                })
                .ToArray();
    }
}