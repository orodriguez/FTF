using System;
using System.Linq;
using FTF.Core.Entities;

namespace FTF.Core.Auth.SignIn
{
    public class Handler
    {
        private readonly IQueryable<User> _users;

        private readonly Action<User> _setCurrentUser;

        public Handler(IQueryable<User> users, Action<User> setCurrentUser)
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