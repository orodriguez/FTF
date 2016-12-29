using System;
using System.Collections.Generic;
using System.Linq;
using FTF.Api.Notes.Retrieve;
using FTF.Core.Extensions;
using FTF.Core.Extensions.Queriable;
using FTF.Core.Queries;

namespace FTF.Core.Notes.Create
{
    public class Handler
    {
        private readonly Func<int> _generateId;

        private readonly Func<DateTime> _getCurrentDate;

        private readonly Action<Note> _saveNote;

        private readonly Action _saveChanges;

        private readonly IQueryable<Tag> _tags;

        private readonly Func<User> _getCurrentUser;

        public Handler(
            Func<int> generateId, 
            Func<DateTime> getCurrentDate, 
            Action<Note> saveNote, 
            Action saveChanges, 
            IQueryable<Tag> tags, 
            Func<User> getCurrentUser)
        {
            _generateId = generateId;
            _getCurrentDate = getCurrentDate;
            _saveNote = saveNote;
            _saveChanges = saveChanges;
            _tags = tags;
            _getCurrentUser = getCurrentUser;
        }

        public void Create(string text)
        {
            _saveNote(new Note
            {
                Id = _generateId(),
                Text = text,
                CreationDate = _getCurrentDate(),
                Tags = MakeTags(text.ParseTagNames()).ToList(),
                User = _getCurrentUser()
            });

            _saveChanges();
        }

        private IEnumerable<ITag> MakeTags(string[] tagNames) => 
            _tags
                .ByName(tagNames)
                .ToArray()
                .Concat(MakeNewTags(tagNames));

        private IEnumerable<ITag> MakeNewTags(string[] tagNames) => 
            tagNames
                .Except(_tags.ByName(tagNames).Names())
                .Select(tagName => new Tag { Name = tagName })
                .ToArray();
    }
}