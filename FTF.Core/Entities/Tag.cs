using System.Collections.Generic;
using System.Linq;
using FTF.Api.Responses;

namespace FTF.Core.Entities
{
    public class Tag : IEntity, INameable, ITag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual User User { get; set; }

        public int NotesCount => Notes.Count();

        public virtual ICollection<Tagging> Taggings { get; set; }

        public IEnumerable<Note> Notes => Taggings.Select(t => t.Note);
    }
}