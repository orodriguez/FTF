using System.Collections.Generic;
using System.Linq;
using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.Extensions;
using FTF.Core.Queries;
using FTF.Core.Storage;

namespace FTF.Core.Notes
{
    [Concrete]
    public class CreateHandler
    {
        private readonly GetCurrentDate _getCurrentDate;

        private readonly IRepository<Note> _notes;

        private readonly IUnitOfWork _uow;

        private readonly IQueryable<Tag> _tags;

        private readonly GetCurrentUser _getCurrentUser;

        private readonly ValidateNote _validate;

        public CreateHandler(
            GetCurrentDate getCurrentDate, 
            IRepository<Note> notes, 
            IUnitOfWork uow, 
            IQueryable<Tag> tags, 
            GetCurrentUser getCurrentUser, 
            ValidateNote validate)
        {
            _getCurrentDate = getCurrentDate;
            _notes = notes;
            _uow = uow;
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

            _notes.Add(note);

            _uow.SaveChanges();

            return note.Id;
        }

        private IEnumerable<Tagging> MakeTaggings(Note note, string[] tagNames) => 
            _tags.Where(t => tagNames.Contains(t.Name))
                .ToArray()
                .Select(tag => new Tagging { Note = note, Tag = tag })
                .Concat(MakeNewTaggings(note, tagNames));

        private IEnumerable<Tagging> MakeNewTaggings(Note note, string[] tagNames) => 
            tagNames
                .Except(_tags.Where(t => tagNames.Contains(t.Name)).Names())
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