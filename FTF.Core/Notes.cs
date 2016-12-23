using System;
using System.Linq;

namespace FTF.Core
{
    public class Notes
    {
        private readonly Func<int> _generateId;

        private readonly Func<DateTime> _getCurrentDate;

        private readonly FtfDbContext _db;

        public Notes(Func<int> generateId, Func<DateTime> getCurrentDate, FtfDbContext db)
        {
            _generateId = generateId;
            _getCurrentDate = getCurrentDate;
            _db = db;
        }

        public void Create(int id, string text)
        {
            _db.Notes.Add(new Note
            {
                Id = _generateId(),
                Text = text,
                CreationDate = _getCurrentDate()
            });

            _db.SaveChanges();
        }

        public Note Retrieve(int id)
        {
            var note = Queryable.FirstOrDefault<Note>(_db.Notes, n => n.Id == id);

            if (note == null)
                throw new Exception($"Note #{id} does not exist");

            return note;
        }
    }
}