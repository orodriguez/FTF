using System.Collections.Generic;
using System.Collections.ObjectModel;
using FTF.Api.Responses;

namespace FTF.Core.Entities
{
    public class Tag : INameable, ITag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Note> Notes { get; set; }

        public virtual User User { get; set; }

        public Tag() : this(new Collection<Note>())
        {
        }

        private Tag(Collection<Note> notes)
        {
            Notes = notes;
        }

        public int NotesCount => Notes.Count;
    }
}