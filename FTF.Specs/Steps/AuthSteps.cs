using System.Data.Entity;
using System.Linq;
using FTF.Core.Auth.SignUp;
using TechTalk.SpecFlow;

namespace FTF.Specs.Steps
{
    [Binding]
    public class AuthSteps
    {
        private readonly Context _context;

        public AuthSteps(Context context)
        {
            _context = context;
        }

        [Given(@"I signup as '(.*)'")]
        public void SignUp(string userName)
        {
            new Handler(
                saveUser: user => _context.Db.Users.Add(user), 
                saveChanges: () => _context.Db.SaveChanges()
            ).SignUp(userName);
        }

        [Given(@"I signin as '(.*)'")]
        public void SignIn(string userName) => 
            _context.CurrentUser = _context.Db.Users.First(u => u.Name == userName);
    }
}
