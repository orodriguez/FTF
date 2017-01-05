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

            return _tags
                .Where(t => t.User.Id == userId)
                .Select(t => new
                {
                    Tag = t,
                    NotesCount = t.Notes.Count(n => n.Tags.All(ta => ta.Name != "Trash"))
                })
                .ToArray()
                .Select(arg => new Response(arg.Tag, arg.NotesCount));
        }

        public IEnumerable<ITag> ListJoint(string tagname)
        {
            var tag = _tags.FirstByName(tagname);

            var notesCount = tag.Notes.Count;

            if (notesCount == 0)
                return new ITag[] { new Response(tag, notesCount) };

            var notesInTag = tag.Notes.Select(n => n.Id);

            var userId = _getCurrentUserId();

            return _tags
                .Where(t => t.User.Id == userId)
                .Where(t => t.Notes.Any(note => notesInTag.Contains(note.Id)))
                .Select(t => new
                {
                    Tag = t,
                    t.Notes,
                    NotesCount = t.Notes.Count(n => notesInTag.Contains(n.Id))
                })
                .ToArray()
                .Select(g => new Response(g.Tag, g.NotesCount));
        }

        private class Response : ITag
        {
            private readonly Tag _tag;

            public Response(Tag tag, int notesCount)
            {
                _tag = tag;
                NotesCount = notesCount;
            }

            public int Id => _tag.Id;

            public string Name => _tag.Name;

            public int NotesCount { get; }
        }
    }
}