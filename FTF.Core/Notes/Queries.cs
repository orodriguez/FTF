using System.Linq;
using FTF.Api.Responses;

namespace FTF.Core.Notes
{
    public delegate int GetCurrentUserId();
    public class Queries
    {
        private readonly IQueryable<Entities.Note> _notes;

        private readonly GetCurrentUserId _getCurrentUserId;

        public Queries(IQueryable<Entities.Note> notes, GetCurrentUserId getCurrentUserId)
        {
            _notes = notes;
            _getCurrentUserId = getCurrentUserId;
        }

        public INote Retrieve(int id)
        {
            var currentUserId = _getCurrentUserId();

            var note = _notes.FirstOrDefault(n => n.Id == id && n.User.Id == currentUserId);

            if (note == null || note.Tags.Any(t => t.Name == "Trash"))
                throw new RecordNotFoundException(id, nameof(Responses.Note));

            return new Responses.Note(note);
        }
    }
}