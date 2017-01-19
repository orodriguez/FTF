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
            var added = TaggingsAdded(note, tagsInText);

            var deleted = TaggingsDeleted(note, tagsInText);

            return new Result(added, deleted);
        }

        private IEnumerable<Tagging> TaggingsAdded(Note note, IEnumerable<string> tagsInText)
        {
            var noteTaggings = _db.Taggings
                .Where(tagging => tagging.Note.Id == note.Id)
                .Select(tagging => tagging.Tag.Name);

            var addedTags = tagsInText
                .Where(n1 => noteTaggings.All(n2 => n1 != n2))
                .ToList();

            var newTags = addedTags
                .Where(name => _db.Tags.All(tag => tag.Name != name))
                .Select(tagName => new Tag
                {
                    Name = tagName,
                    User = _getCurrentUser()
                });

            return newTags
                .Concat(ExistingTags(addedTags))
                .Select(t => _taggingsFactory.Make(note, t));
        }

        private static IEnumerable<Tagging> TaggingsDeleted(Note note, IEnumerable<string> tagsInText) => 
            note.Taggings.Where(t => !tagsInText.Contains(t.Tag.Name));

        private IEnumerable<Tag> ExistingTags(IEnumerable<string> addedTags) => 
            _db.Tags
                .Where(tag => addedTags.Any(tagName => tag.Name == tagName))
                .ToList();

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