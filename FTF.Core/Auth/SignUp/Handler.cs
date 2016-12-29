using System;

namespace FTF.Core.Auth.SignUp
{
    public class Handler
    {
        private readonly Action<User> _saveUser;

        private readonly Action _saveChanges;

        public Handler(
            Action<User> saveUser, 
            Action saveChanges)
        {
            _saveUser = saveUser;
            _saveChanges = saveChanges;
        }

        public void SignUp(string userName)
        {
            _saveUser(new User
            {
                Name = userName
            });

            _saveChanges();
        }
    }
}