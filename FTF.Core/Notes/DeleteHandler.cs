using System.Linq;
using FTF.Core.Attributes;
using FTF.Core.Entities;
using FTF.Core.Storage;

namespace FTF.Core.Notes
{
    [Concrete]
    public class DeleteHandler
    {
        private readonly IQueryable<Note> _notes;

        private readonly IQueryable<Tag> _tags;

        private readonly IUnitOfWork _uow;

        public DeleteHandler(
            IQueryable<Note> notes, 
            IQueryable<Tag> tags, 
            IUnitOfWork uow)
        {
            _notes = notes;
            _tags = tags;
            _uow = uow;
        }

        public void Delete(int id)
        {
            var note = _notes.First(n => n.Id == id);

            var trashTag = _tags.FirstOrDefault(t => t.Name == "Trash") ?? new Tag { Name = "Trash" };

            note.Taggings.Add(new Tagging
            {
                Note = note,
                Tag = trashTag
            });

            _uow.SaveChanges();
        }
    }
}