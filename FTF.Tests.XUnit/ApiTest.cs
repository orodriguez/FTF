using System;
using FTF.Api;
using FTF.Api.Actions.Auth;
using FTF.Api.Responses;
using FTF.IoC.SimpleInjector;

namespace FTF.Tests.XUnit
{
    public class ApiTest
    {
        private DateTime _currentTime;

        private readonly IContainer _container;

        public ApiTest(IContainer container)
        {
            _container = ContainerFactory.Make();
        }

        protected void SignIn(string username)
        {
            var signIn = _container.GetInstance<SignIn>();
            signIn(username);
        }

        protected void SignUp(string username)
        {
            throw new System.NotImplementedException();
        }

        protected void TodayIs(int year, int month, int day)
        {
            _currentTime = new DateTime(year, month, day);
        }

        protected int CreateNotes(string text)
        {
            throw new System.NotImplementedException();
        }

        protected INote RetrieveNote(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}