using FTF.Api.Actions.Auth;
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
            SignUp signUp = new Core.Auth.SignUp.Handler(
                saveUser: user => _context.Db.Users.Add(user), 
                saveChanges: () => _context.Db.SaveChanges()
            ).SignUp;

            signUp(userName);
        }

        [Given(@"I signin as '(.*)'")]
        public void SignIn(string userName)
        {
            SignIn signIn = new Core.Auth.SignIn.Handler(
                users: _context.Db.Users, 
                setCurrentUser: user => _context.CurrentUser = user
            ).SignIn;

            signIn(userName);
        }
    }
}
