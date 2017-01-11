namespace FTF.Tests.XUnit
{
    public class UserAuthenticatedTest : ApplicationTestBase
    {
        public UserAuthenticatedTest()
        {
            App.Auth.SignUp("orodriguez");
            App.Auth.SignIn("orodriguez");
        }
    }
}