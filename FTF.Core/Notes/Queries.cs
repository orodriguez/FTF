using System;
using System.Linq;
using FTF.Api.Responses;
using FTF.Core.Entities;
using FTF.Core.Responses;
using Note = FTF.Core.Responses.Note;

namespace FTF.Core.Notes
{
    public class Queries
    {
        private readonly IQueryable<Entities.Note> _notes;

        private readonly Func<int> _getCurrentUserId;

        public Queries(IQueryable<Entities.Note> notes, Func<int> getCurrentUserId)
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

            return new Note(note);
        }
    }
}