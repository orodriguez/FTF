namespace FTF.Api.Services
{
    public interface IAuthService
    {
        void SignUp(string username);
        void SignIn(string username);
    }
}