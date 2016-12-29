using System.Collections.Generic;
using System.Collections.ObjectModel;
using FTF.Api.Responses.Notes.Retrieve;

namespace FTF.Core.Entities
{
    public class Tag : INameable, ITag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Note> Notes { get; set; }

        public Tag() : this(new Collection<Note>())
        {
        }

        public int NotesCount => Notes.Count;

        private Tag(Collection<Note> notes)
        {
            Notes = notes;
        }
    }
}