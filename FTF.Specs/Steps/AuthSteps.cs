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
        public void SignUp(string userName) => 
            _context.Db.Users.Add(new User
            {
                Name = userName
            });
    }
}
