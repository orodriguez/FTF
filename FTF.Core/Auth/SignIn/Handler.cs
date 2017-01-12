using System.Linq;
using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.EntityFramework;

namespace FTF.Core.Auth.SignIn
{
    [Concrete]
    public class Handler
    {
        private readonly DbContext _db;

        private readonly SetCurrentUser _setCurrentUser;

        public Handler(DbContext db, SetCurrentUser setCurrentUser)
        {
            _db = db;
            _setCurrentUser = setCurrentUser;
        }

        public void SignIn(string username)
        {
            var user = _db.Users.First(u => u.Name == username);

            _setCurrentUser(user);
        }
    }
}