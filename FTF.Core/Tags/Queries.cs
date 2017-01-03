using System;
using System.Collections.Generic;
using System.Linq;
using FTF.Api.Responses;
using FTF.Core.Entities;
using FTF.Core.Extensions.Queriable;

namespace FTF.Core.Tags
{
    public class Queries
    {
        private readonly IQueryable<Tag> _tags;

        private readonly Func<int> _getCurrentUserId;

        public Queries(IQueryable<Tag> tags, Func<int> getCurrentUserId)
        {
            _tags = tags;
            _getCurrentUserId = getCurrentUserId;
        }

        public IEnumerable<ITag> ListAll()
        {
            var userId = _getCurrentUserId();

            return _tags.Where(t => t.User.Id == userId);
        }

        public IEnumerable<ITag> ListJoint(string tagname)
        {
            var notesInTag = _tags.FirstByName(tagname).Notes.Select(n => n.Id);

            var userId = _getCurrentUserId();

            return _tags
                .Where(t => t.User.Id == userId)
                .Where(t => t.Notes.Any(note => notesInTag.Contains(note.Id)))
                .ToArray()
                .Select(tag => new TagJoint(tag, notesInTag));
        }

        public class TagJoint : ITag
        {
            private readonly Tag _tag;

            private readonly IEnumerable<int> _countFilter;

            public TagJoint(Tag tag, IEnumerable<int> countFilter)
            {
                _tag = tag;
                _countFilter = countFilter;
            }

            public int Id => _tag.Id;

            public string Name => _tag.Name;

            public int NotesCount => _tag.Notes.Count(note => _countFilter.Contains(note.Id));
        }
    }
}