using System.Collections.Generic;
using System.Linq;
using FTF.Api.Responses;
using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.EntityFramework;

namespace FTF.Core.Tags
{
    [Concrete]
    public class Queries
    {
        private readonly DbContext _db;

        private readonly GetCurrentUser _getCurrentUser;

        public Queries(DbContext db, GetCurrentUser getCurrentUser)
        {
            _getCurrentUser = getCurrentUser;
            _db = db;
        }

        public IEnumerable<ITag> ListAll()
        {
            var userId = _getCurrentUser().Id;

            var notesInTrash = _db.Taggings
                .Where(Tagging.Trash)
                .Select(tn => tn.Note.Id)
                .ToArray();

            return _db.Taggings
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

        public IEnumerable<ITag> ListJoint(string tagname)
        {
            var userId = _getCurrentUser().Id;

            var notesInTrash = _db.Taggings
                .Where(Tagging.Trash)
                .Select(tn => tn.Note.Id)
                .ToArray();

            var notesWithTag = _db.Taggings
                .Where(t => t.Tag.Name == tagname)
                .Select(t => t.Note.Id)
                .ToArray();

            return _db.Taggings
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