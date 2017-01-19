using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.EntityFramework;
using FTF.Core.Extensions;
using FTF.Core.Factories;
using static FTF.Core.Predicates.GenericPredicates;

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

        public Result DiffTaggings(Note note, string text) => 
            DiffTaggings(note, text.ParseTagNames());

        private Result DiffTaggings(Note note, string[] tagsInText)
        {
            var noteTaggings = _db.Taggings
                .Where(tagging => tagging.Note.Id == note.Id)
                .Select(tagging => tagging.Tag.Name);

            var addedTags = tagsInText
                .Where(NotIn(noteTaggings))
                .ToList();

            var tagsToCreate = addedTags.Where(name => _db.Tags.All(tag => tag.Name != name));

            var newTags = tagsToCreate.Select(tagName => new Tag
            {
                Name = tagName,
                User = _getCurrentUser()
            });

            var taggingsOfNewTags = newTags
                .Select(tag => _taggingsFactory.Make(note, tag));

            var taggingsToDelete = note.Taggings.Where(t => !tagsInText.Contains(t.Tag.Name));

            var existingTaggings = ExistingTags(note, tagsInText)
                .Select(tag => _taggingsFactory.Make(note, tag));

            var taggingsToAdd = taggingsOfNewTags.Concat(existingTaggings);

            return new Result(taggingsToAdd, taggingsToDelete);
        }

        private IEnumerable<Tag> ExistingTags(Note note, IEnumerable<string> tagsInText)
        {
            var noteTaggings = _db.Taggings
                .Where(tagging => tagging.Note.Id == note.Id)
                .Select(tagging => tagging.Tag.Name);

            var addedTags = tagsInText
                .Where(NotIn(noteTaggings))
                .ToList();

            return _db.Tags
                .Where(tag => addedTags.Any(tagName => tag.Name == tagName))
                .ToList();
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