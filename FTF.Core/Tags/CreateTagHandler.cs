using FTF.Core.Attributes;
using FTF.Core.Entities;
using FTF.Core.EntityFramework;

namespace FTF.Core.Tags
{
    [Concrete]
    public class CreateTagHandler
    {
        private readonly DbContext _db;

        public CreateTagHandler(DbContext db)
        {
            _db = db;
        }

        public void Create(string name)
        {
            _db.Tags.Add(new Tag
            {
                Name = name
            });

            _db.SaveChanges();
        }
    }
}