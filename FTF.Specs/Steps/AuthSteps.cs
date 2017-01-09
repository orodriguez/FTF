using FTF.Api.Actions.Auth;
using TechTalk.SpecFlow;

namespace FTF.Specs.Steps
{
    [Binding]
    public class AuthSteps
    {
        private readonly SignUp _signUp;

        private readonly SignIn _signIn;

        public AuthSteps(SignUp signUp, SignIn signIn)
        {
            _signUp = signUp;
            _signIn = signIn;
        }

        [Given(@"I signup as '(.*)'")]
        public void SignUp(string userName) => _signUp(userName);

        [Given(@"I signin as '(.*)'")]
        public void SignIn(string userName) => _signIn(userName);
        
        [Given(@"I signup and signin as '(.*)'")]
        public void SignUpAndSignIn(string userName)
        {
            SignUp(userName);
            SignIn(userName);
        }
    }
}
