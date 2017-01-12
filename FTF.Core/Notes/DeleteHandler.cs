using System.Linq;
using FTF.Core.Attributes;
using FTF.Core.Entities;
using FTF.Core.EntityFramework;

namespace FTF.Core.Notes
{
    [Concrete]
    public class DeleteHandler
    {
        private DbContext _db;

        public DeleteHandler(DbContext db)
        {
            _db = db;
        }

        public void Delete(int id)
        {
            var note = _db.Notes.First(n => n.Id == id);

            var trashTag = _db.Tags.FirstOrDefault(t => t.Name == "Trash") ?? new Tag { Name = "Trash" };

            note.Taggings.Add(new Tagging
            {
                Note = note,
                Tag = trashTag
            });

            _db.SaveChanges();
        }
    }
}