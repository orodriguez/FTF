using System;
using System.Linq;
using FTF.Api.Responses;
using FTF.Core.Entities;
using FTF.Core.Notes.Retrieve;

namespace FTF.Core.Notes
{
    public class Queries
    {
        private readonly IQueryable<Note> _notes;

        private readonly Func<int> _getCurrentUserId;

        public Queries(IQueryable<Note> notes, Func<int> getCurrentUserId)
        {
            _notes = notes;
            _getCurrentUserId = getCurrentUserId;
        }

        public INote Retrieve(int id)
        {
            var currentUserId = _getCurrentUserId();

            var note = _notes.FirstOrDefault(n => n.Id == id && n.User.Id == currentUserId);

            if (note == null)
                throw new Exception($"Note #{id} does not exist");

            return new Response(note);
        }
    }
}