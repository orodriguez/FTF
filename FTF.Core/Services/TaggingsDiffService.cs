using System.Collections.Generic;
using System.Linq;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.EntityFramework;
using FTF.Core.Extensions;
using FTF.Core.Factories;

namespace FTF.Core.Services
{
    public class TaggingsDiffService
    {
        private readonly DbContext _db;

        private readonly GetCurrentUser _getCurrentUser;

        private readonly TaggingsFactory _taggingsFactory;

        public TaggingsDiffService(DbContext db, 
            GetCurrentUser getCurrentUser, 
            TaggingsFactory taggingsFactory)
        {
            _db = db;
            _getCurrentUser = getCurrentUser;
            _taggingsFactory = taggingsFactory;
        }

        public Result DiffTaggings(Note note, string text)
        {
            var tagNamesInText = text.ParseTagNames();

            var existingTaggings = _db.Taggings
                .Where(tagging => tagging.Note.Id == note.Id);

            var existingTaggingsNames = existingTaggings.Select(tagging => tagging.Tag.Name);

            var difference = tagNamesInText.Except(existingTaggingsNames);

            var addedTags = difference
                .Where(t1 => existingTaggingsNames.All(t2 => t1 != t2))
                .ToList();

            var existingTags = _db.Tags
                .Where(tag => addedTags.Any(tagName => tag.Name == tagName))
                .ToList();

            var taggingsOfExistingTags = existingTags
                .Select(tag => _taggingsFactory.Make(note, tag));

            var tagsToCreate = addedTags.Except(existingTags.Select(t => t.Name));

            var newTags = tagsToCreate.Select(tagName => new Tag
            {
                Name = tagName,
                User = _getCurrentUser()
            });

            var taggingsOfNewTags = newTags
                .Select(tag => _taggingsFactory.Make(note, tag));

            var taggingsToDelete = note.Taggings.Where(t => !tagNamesInText.Contains(t.Tag.Name));

            var taggingsToAdd = taggingsOfNewTags.Concat(taggingsOfExistingTags);

            return new Result(taggingsToAdd, taggingsToDelete);
        }

        public class Result
        {
            public IEnumerable<Tagging> Added { get; }

            public IEnumerable<Tagging> Deleted { get; }

            public Result(IEnumerable<Tagging> added, IEnumerable<Tagging> deleted)
            {
                Added = added;
                Deleted = deleted;
            }
        }
    }
}