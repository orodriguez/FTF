using System;
using System.Collections.Generic;
using System.Linq;
using FTF.Api.Responses;
using FTF.Core.Entities;

namespace FTF.Core.Tags
{
    public class Queries
    {
        private readonly IQueryable<Tagging> _taggedTaggetNotes;

        private readonly Func<int> _getCurrentUserId;

        public Queries(IQueryable<Tagging> taggetNotes, Func<int> getCurrentUserId)
        {
            _taggedTaggetNotes = taggetNotes;
            _getCurrentUserId = getCurrentUserId;
        }

        public IEnumerable<ITag> ListAll()
        {
            var userId = _getCurrentUserId();

            var notesInTrash = _taggedTaggetNotes
                .Where(t => t.Note.User.Id == userId)
                .Where(t => t.Tag.Name == "Trash")
                .Select(tn => tn.Note.Id)
                .ToArray();

            return _taggedTaggetNotes
                .Where(tn => tn.Tag.User.Id == userId)
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
            throw new NotImplementedException();
            /*var tag = _taggedTaggetNotes.FirstByName(tagname);

            var notesCount = tag.Notes.Count;

            if (notesCount == 0)
                return new ITag[] { new Response(tag, notesCount) };

            var notesInTag = tag.Notes.Select(n => n.Id);

            var userId = _getCurrentUserId();

            return _taggedTaggetNotes
                .Where(t => t.User.Id == userId)
                .Where(t => t.Notes.Any(note => notesInTag.Contains(note.Id)))
                .Select(t => new
                {
                    Tag = t,
                    t.Notes,
                    NotesCount = t.Notes.Count(n => notesInTag.Contains(n.Id))
                })
                .ToArray()
                .Select(g => new Response(g.Tag, g.NotesCount));*/
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