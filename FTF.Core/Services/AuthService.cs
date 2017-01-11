using FTF.Api;
using FTF.Api.Actions.Auth;

namespace FTF.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignUp _signUp;

        private readonly SignIn _signIn;

        public AuthService(SignUp signUp, SignIn signIn)
        {
            _signUp = signUp;
            _signIn = signIn;
        }

        public void SignUp(string username) => _signUp(username);

        public void SignIn(string username) => _signIn(username);
    }
}