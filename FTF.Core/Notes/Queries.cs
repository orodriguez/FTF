using System;
using System.Linq;

namespace FTF.Core.Notes
{
    public class Queries
    {
        private readonly IQueryable<Note> _notes;

        public Queries(IQueryable<Note> notes)
        {
            _notes = notes;
        }

        public Note Retrieve(int id)
        {
            var note = _notes.FirstOrDefault<Note>(n => n.Id == id);

            if (note == null)
                throw new Exception($"Note #{id} does not exist");

            return note;
        }
    }
}