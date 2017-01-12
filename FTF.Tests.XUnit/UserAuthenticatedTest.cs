namespace FTF.Tests.XUnit
{
    public class UserAuthenticatedTest : ApplicationTest
    {
        public UserAuthenticatedTest()
        {
            App.Auth.SignUp("orodriguez");
            App.Auth.SignIn("orodriguez");
        }
    }
}