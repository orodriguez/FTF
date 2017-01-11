using System.Collections.Generic;
using System.Linq;
using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.Extensions;
using FTF.Core.Extensions.Queriable;
using FTF.Core.Queries;

namespace FTF.Core.Notes
{
    [Concrete]
    public class CreateHandler
    {
        private readonly GetCurrentDate _getCurrentDate;

        private readonly Save<Note> _saveNote;

        private readonly SaveChanges _saveChanges;

        private readonly IQueryable<Tag> _tags;

        private readonly GetCurrentUser _getCurrentUser;

        private readonly ValidateNote _validate;

        public CreateHandler(
            GetCurrentDate getCurrentDate, 
            Save<Note> saveNote, 
            SaveChanges saveChanges, 
            IQueryable<Tag> tags, 
            GetCurrentUser getCurrentUser, 
            ValidateNote validate)
        {
            _getCurrentDate = getCurrentDate;
            _saveNote = saveNote;
            _saveChanges = saveChanges;
            _tags = tags;
            _getCurrentUser = getCurrentUser;
            _validate = validate;
        }

        public int Create(string text)
        {
            _validate(text);

            var note = new Note
            {
                Text = text,
                CreationDate = _getCurrentDate(),
                User = _getCurrentUser()
            };

            note.Taggings = MakeTaggings(note, text.ParseTagNames()).ToList();

            _saveNote(note);

            _saveChanges();

            return note.Id;
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