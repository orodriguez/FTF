using FTF.Core.Attributes;
using FTF.Core.Entities;
using FTF.Core.EntityFramework;

namespace FTF.Core.Auth.SignUp
{
    [Concrete]
    public class Handler
    {
        private readonly DbContext _db;

        public Handler(DbContext db)
        {
            _db = db;
        }

        public void SignUp(string userName)
        {
            _db.Users.Add(new User
            {
                Name = userName
            });

            _db.SaveChanges();
        }
    }
}