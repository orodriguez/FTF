using System.Linq;
using FTF.Core;
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
            _context.Db.Users.Add(new User
            {
                Name = userName
            });

            _context.Db.SaveChanges();
        }

        [Given(@"I signin as '(.*)'")]
        public void SignIn(string userName) => 
            _context.CurrentUser = _context.Db.Users.First(u => u.Name == userName);
    }
}
