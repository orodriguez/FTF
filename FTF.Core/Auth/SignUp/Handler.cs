using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.Entities;

namespace FTF.Core.Auth.SignUp
{
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

        [Action(typeof(Api.Actions.Auth.SignUp))]
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