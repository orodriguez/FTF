using System.Linq;
using FTF.Api.Exceptions;
using FTF.Api.Responses;
using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.EntityFramework;

namespace FTF.Core.Notes
{
    [Concrete]
    public class Queries
    {
        private readonly DbContext _db;

        private readonly GetCurrentUser _getCurrentUser;

        public Queries(DbContext db, GetCurrentUser getCurrentUser)
        {
            _db = db;
            _getCurrentUser = getCurrentUser;
        }

        public INote Retrieve(int id)
        {
            var currentUserId = _getCurrentUser().Id;

            var note = _db.Notes.FirstOrDefault(n => n.Id == id && n.User.Id == currentUserId);

            if (note == null || note.Tags.Any(t => t.Name == "Trash"))
                throw new RecordNotFoundException(id, nameof(Responses.Note));

            return new Responses.Note(note);
        }
    }
}