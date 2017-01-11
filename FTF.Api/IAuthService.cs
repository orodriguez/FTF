namespace FTF.Api
{
    public interface IAuthService
    {
        void SignUp(string username);
        void SignIn(string username);
    }
}