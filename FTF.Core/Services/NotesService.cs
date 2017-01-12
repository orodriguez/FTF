using System.Collections.Generic;
using System.Linq;
using FTF.Api.Exceptions;
using FTF.Api.Responses;
using FTF.Api.Services;
using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.EntityFramework;
using FTF.Core.Extensions;
using FTF.Core.Queries;

namespace FTF.Core.Services
{
    [Role(typeof(INotesService))]
    public class NotesService : INotesService
    {
        private readonly GetCurrentDate _getCurrentDate;

        private readonly DbContext _db;

        private readonly GetCurrentUser _getCurrentUser;

        public NotesService(DbContext db, 
            GetCurrentDate getCurrentDate, 
            GetCurrentUser getCurrentUser)
        {
            _db = db;
            _getCurrentDate = getCurrentDate;
            _getCurrentUser = getCurrentUser;
        }

        public int Create(string text)
        {
            Validate(text);

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

        public INote Retrieve(int id)
        {
            var currentUserId = _getCurrentUser().Id;

            var note = _db.Notes.FirstOrDefault(n => n.Id == id && n.User.Id == currentUserId);

            if (note == null || note.Tags.Any(t => t.Name == "Trash"))
                throw new RecordNotFoundException(id, nameof(Responses.Note));

            return new Responses.Note(note);
        }

        public void Update(int id, string text)
        {
            Validate(text);

            var noteToUpdate = _db.Notes.First(n => n.Id == id);

            noteToUpdate.Text = text;

            noteToUpdate.Taggings = MakeTaggings(noteToUpdate, text.ParseTagNames()).ToList();

            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var note = _db.Notes.First(n => n.Id == id);

            var trashTag = _db.Tags.FirstOrDefault(t => t.Name == "Trash") ?? new Tag { Name = "Trash" };

            note.Taggings.Add(new Tagging
            {
                Note = note,
                Tag = trashTag
            });

            _db.SaveChanges();
        }

        public static void Validate(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ValidationException("Note can not be empty");
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