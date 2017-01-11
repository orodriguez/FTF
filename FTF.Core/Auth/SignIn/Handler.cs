using System.Linq;
using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.Entities;

namespace FTF.Core.Auth.SignIn
{
    [Concrete]
    public class Handler
    {
        private readonly IQueryable<User> _users;

        private readonly SetCurrentUser _setCurrentUser;

        public Handler(IQueryable<User> users, SetCurrentUser setCurrentUser)
        {
            _users = users;
            _setCurrentUser = setCurrentUser;
        }

        public void SignIn(string username)
        {
            var user = _users.First(u => u.Name == username);

            _setCurrentUser(user);
        }
    }
}