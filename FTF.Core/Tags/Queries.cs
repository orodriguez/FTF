using System.Collections.Generic;
using System.Linq;
using FTF.Api.Responses;
using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.Delegates.Actions.Tags;
using FTF.Core.Entities;

namespace FTF.Core.Tags
{
    [Concrete]
    public class Queries
    {
        private readonly IQueryable<Tagging> _taggings;

        private readonly GetCurrentUserId _getCurrentUserId;

        public Queries(IQueryable<Tagging> taggetNotes, GetCurrentUserId getCurrentUserId)
        {
            _taggings = taggetNotes;
            _getCurrentUserId = getCurrentUserId;
        }

        [Role(typeof(ListAll))]
        public IEnumerable<ITag> ListAll()
        {
            var userId = _getCurrentUserId();

            var notesInTrash = _taggings
                .Where(Tagging.Trash)
                .Select(tn => tn.Note.Id)
                .ToArray();

            return _taggings
                .Where(Tagging.TagCreatedByUser(userId))
                .GroupBy(tn => tn.Tag)
                .Select(g => new
                {
                    Tag = g.Key,
                    NotesCount = g.Count(tagging => !notesInTrash.Contains(tagging.Note.Id))
                })
                .ToArray()
                .Select(t => new Response(t.Tag, t.NotesCount));
        }

        [Role(typeof(ListJoint))]
        public IEnumerable<ITag> ListJoint(string tagname)
        {
            var userId = _getCurrentUserId();

            var notesInTrash = _taggings
                .Where(Tagging.Trash)
                .Select(tn => tn.Note.Id)
                .ToArray();

            var notesWithTag = _taggings
                .Where(t => t.Tag.Name == tagname)
                .Select(t => t.Note.Id)
                .ToArray();

            return _taggings
                .Where(tn => tn.Tag.User.Id == userId)
                .Where(tn => notesWithTag.Contains(tn.Note.Id))
                .GroupBy(tn => tn.Tag)
                .Select(g => new
                {
                    Tag = g.Key,
                    NotesCount = g.Count(tagging => !notesInTrash.Contains(tagging.Note.Id))
                })
                .ToArray()
                .Select(t => new Response(t.Tag, t.NotesCount));
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