using FTF.Api.Services;
using FTF.Core.Attributes;

namespace FTF.Core.Services
{
    [Role(typeof(IAuthService))]
    public class AuthService : IAuthService
    {
        private readonly Auth.SignUp.Handler _signUp;

        private readonly Auth.SignIn.Handler _signIn;

        public AuthService(Auth.SignUp.Handler signUp, Auth.SignIn.Handler signIn)
        {
            _signUp = signUp;
            _signIn = signIn;
        }

        public void SignUp(string username) => _signUp.SignUp(username);

        public void SignIn(string username) => _signIn.SignIn(username);
    }
}