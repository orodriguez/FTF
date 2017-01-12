using FTF.Core.Attributes;
using FTF.Core.Entities;
using FTF.Core.Storage;

namespace FTF.Core.Auth.SignUp
{
    [Concrete]
    public class Handler
    {
        private readonly IRepository<User> _users;

        private readonly IUnitOfWork _uow;

        public Handler(
            IRepository<User> users, 
            IUnitOfWork uow)
        {
            _users = users;
            _uow = uow;
        }

        public void SignUp(string userName)
        {
            _users.Add(new User
            {
                Name = userName
            });

            _uow.SaveChanges();
        }
    }
}