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

        public NotesService(DbContext db, 
            GetCurrentTime getCurrentTime, 
            GetCurrentUser getCurrentUser)
        {
            _db = db;
            _getCurrentTime = getCurrentTime;
            _getCurrentUser = getCurrentUser;
        }

        public int Create(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ValidationException("Note can not be empty");

            var note = new Note
            {
                Text = text,
                CreationDate = _getCurrentTime(),
                User = _getCurrentUser(),
                Taggings = new List<Tagging>()
            };

            // Existing tags

            var tagNamesInText = text.ParseTagNames();

            var existingTags = _db.Tags.Where(t => tagNamesInText.Contains(t.Name));

            var taggingsForExistingTags = existingTags
                .ToList()
                .Select(tag => new Tagging
                {
                    CreationDate = _getCurrentTime(),
                    Tag = tag
                });

            taggingsForExistingTags
                .ToList()
                .ForEach(t => note.Taggings.Add(t));

            // New tags

            var newTagNames = tagNamesInText.Except(existingTags.Select(t => t.Name));

            var newTags = newTagNames.Select(tagName => new Tag
            {
                Name = tagName,
                User = _getCurrentUser()
            });

            var taggingsForNewTags = newTags.Select(t => new Tagging
            {
              Note  = note,
              Tag = t,
              CreationDate = _getCurrentTime()
            });
            
            taggingsForNewTags
                .ToList()
                .ForEach(t => note.Taggings.Add(t));

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

                var tagNamesInText = text.ParseTagNames();

                var tagNamesAlreadyTagged = noteToUpdate.Tags.Select(t => t.Name).ToList();

                var difference = tagNamesInText.Except(tagNamesAlreadyTagged);

                var newTagNames = difference.Where(t1 => tagNamesAlreadyTagged.All(t2 => t1 != t2));

                var newTags = newTagNames.Select(tagName => new Tag
                {
                    Name = tagName,
                    User = _getCurrentUser()
                });

                var taggingsOfNewTags = newTags.Select(tag => new Tagging
                {
                    Note = noteToUpdate,
                    Tag = tag,
                    CreationDate = _getCurrentTime(),
                });

                taggingsOfNewTags.ToList().ForEach(t => noteToUpdate.Taggings.Add(t));
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