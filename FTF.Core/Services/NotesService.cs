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

namespace FTF.Core.Services
{
    [Role(typeof(INotesService))]
    public class NotesService : INotesService
    {
        private readonly GetCurrentTime _getCurrentTime;

        private readonly DbContext _db;

        private readonly GetCurrentUser _getCurrentUser;

        private readonly TaggingsDiffService _taggingDiffService;

        public NotesService(DbContext db, 
            GetCurrentTime getCurrentTime, 
            GetCurrentUser getCurrentUser, 
            TaggingsDiffService taggingDiffService)
        {
            _db = db;
            _getCurrentTime = getCurrentTime;
            _getCurrentUser = getCurrentUser;
            _taggingDiffService = taggingDiffService;
        }

        public int Create(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ValidationException("Note can not be empty");

            var note = new Note
            {
                Text = text,
                CreationDate = _getCurrentTime(),
                User = _getCurrentUser()
            };

            note.Taggings = _taggingDiffService
                .TaggingsAdded(note, text)
                .ToList();

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
            if (text != null && text.Trim() == "")
                throw new ValidationException("Note can not be empty");

            var noteToUpdate = _db.Notes.First(n => n.Id == id);

            if (text != null)
            {
                noteToUpdate.Text = text;

                var result = _taggingDiffService.DiffTaggings(noteToUpdate, text);

                result.Added
                    .ToList()
                    .ForEach(tagging => _db.Taggings.Add(tagging));

                result.Deleted
                    .ToList()
                    .ForEach(tagging => _db.Taggings.Remove(tagging));
            }

            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var note = _db.Notes.First(n => n.Id == id);

            var trashTag = _db.Tags.FirstOrDefault(t => t.Name == "Trash") ?? new Tag { Name = "Trash" };

            note.Taggings.Add(new Tagging
            {
                Note = note,
                Tag = trashTag,
                CreationDate = _getCurrentTime()
            });

            _db.SaveChanges();
        }

        public IEnumerable<INote> All() => 
            _db.Notes
                .OrderByDescending(n => n.CreationDate)
                .ToList()
                .Select(n => new Responses.Note(n));

        public IEnumerable<INote> ByTag(string tagName)
        {
            var tag = _db.Tags.First(t => t.Name == tagName);

            var notes = _db.Taggings
                .Where(t => t.Tag.Id == tag.Id)
                .OrderByDescending(t => t.CreationDate)
                .Select(t => t.Note);

            return notes.ToList().Select(n => new Responses.Note(n));
        }
    }
}