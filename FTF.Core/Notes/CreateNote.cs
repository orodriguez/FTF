using System;

namespace FTF.Core.Notes
{
    public class CreateNote
    {
        private readonly Func<int> _generateId;

        private readonly Func<DateTime> _getCurrentDate;

        private readonly FtfDbContext _db;

        public CreateNote(Func<int> generateId, Func<DateTime> getCurrentDate, FtfDbContext db)
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
    }
}