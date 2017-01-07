using FTF.Api.Actions.Auth;
using TechTalk.SpecFlow;

namespace FTF.Specs.Steps
{
    [Binding]
    public class AuthSteps : Steps
    {
        public AuthSteps(Context context) : base(context)
        {
        }

        [Given(@"I signup as '(.*)'")]
        public void SignUp(string userName) => Exec<SignUp>(f => f(userName));

        [Given(@"I signin as '(.*)'")]
        public void SignIn(string userName) => Exec<SignIn>(f => f(userName));
        
        [Given(@"I signup and signin as '(.*)'")]
        public void SignUpAndSignIn(string userName)
        {
            SignUp(userName);
            SignIn(userName);
        }
    }
}
