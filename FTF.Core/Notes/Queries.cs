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

        public Queries(IQueryable<Note> notes)
        {
            _notes = notes;
        }

        public INote Retrieve(int id)
        {
            var note = _notes.FirstOrDefault(n => n.Id == id);

            if (note == null)
                throw new Exception($"Note #{id} does not exist");

            return new Response(note);
        }
    }
}