using System.Linq;
using FTF.Api.Actions.Notes;
using FTF.Api.Responses;
using FTF.Core.Attributes;
using FTF.Core.Delegates;

namespace FTF.Core.Notes
{
    [Concrete]
    public class Queries
    {
        private readonly IQueryable<Entities.Note> _notes;

        private readonly GetCurrentUserId _getCurrentUserId;

        public Queries(IQueryable<Entities.Note> notes, GetCurrentUserId getCurrentUserId)
        {
            _notes = notes;
            _getCurrentUserId = getCurrentUserId;
        }

        [Delegate(typeof(Retrieve))]
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