using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.Entities;

namespace FTF.Core.Auth.SignUp
{
    [Concrete]
    public class Handler
    {
        private readonly Save<User> _saveUser;

        private readonly SaveChanges _saveChanges;

        public Handler(
            Save<User> saveUser, 
            SaveChanges saveChanges)
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