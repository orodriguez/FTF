using System.Linq;
using FTF.Api.Services;
using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.EntityFramework;

namespace FTF.Core.Services
{
    [Role(typeof(IAuthService))]
    public class AuthService : IAuthService
    {
        private readonly DbContext _db;

        private readonly SetCurrentUser _setCurrentUser;

        public AuthService(DbContext db, SetCurrentUser setCurrentUser)
        {
            _db = db;
            _setCurrentUser = setCurrentUser;
        }

        public void SignUp(string userName)
        {
            _db.Users.Add(new User
            {
                Name = userName
            });

            _db.SaveChanges();
        }

        public void SignIn(string username)
        {
            var user = _db.Users.First(u => u.Name == username);

            _setCurrentUser(user);
        }
    }
}