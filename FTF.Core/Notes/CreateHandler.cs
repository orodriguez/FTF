using System.Collections.Generic;
using System.Linq;
using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.EntityFramework;
using FTF.Core.Extensions;
using FTF.Core.Queries;

namespace FTF.Core.Notes
{
    [Concrete]
    public class CreateHandler
    {
        private readonly GetCurrentDate _getCurrentDate;

        private readonly DbContext _db;

        private readonly GetCurrentUser _getCurrentUser;

        public CreateHandler(
            GetCurrentDate getCurrentDate, 
            DbContext db, 
            GetCurrentUser getCurrentUser)
        {
            _getCurrentDate = getCurrentDate;
            _getCurrentUser = getCurrentUser;
            _db = db;
        }

        public int Create(string text)
        {
            NoteValidator.Validate(text);

            var note = new Note
            {
                Text = text,
                CreationDate = _getCurrentDate(),
                User = _getCurrentUser()
            };

            note.Taggings = MakeTaggings(note, text.ParseTagNames()).ToList();

            _db.Notes.Add(note);

            _db.SaveChanges();

            return note.Id;
        }

        private IEnumerable<Tagging> MakeTaggings(Note note, string[] tagNames) => 
            _db.Tags.Where(t => tagNames.Contains(t.Name))
                .ToArray()
                .Select(tag => new Tagging { Note = note, Tag = tag })
                .Concat(MakeNewTaggings(note, tagNames));

        private IEnumerable<Tagging> MakeNewTaggings(Note note, string[] tagNames) => 
            tagNames
                .Except(_db.Tags.Where(t => tagNames.Contains(t.Name)).Names())
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