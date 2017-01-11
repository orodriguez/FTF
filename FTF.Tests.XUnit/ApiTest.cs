using System;
using FTF.Api;
using FTF.Api.Actions.Auth;
using FTF.Api.Responses;
using FTF.Core.Delegates;
using FTF.Core.Ports;
using FTF.IoC.SimpleInjector;

namespace FTF.Tests.XUnit
{
    public class ApiTest
    {
        private DateTime _currentDate;

        private readonly IContainer _container;

        public ApiTest()
        {
            _container = ContainerFactory.Make(new Adapter(
                getCurrentDate: () => _currentDate));
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
            _currentDate = new DateTime(year, month, day);
        }

        protected int CreateNotes(string text)
        {
            throw new System.NotImplementedException();
        }

        protected INote RetrieveNote(int id)
        {
            throw new System.NotImplementedException();
        }

        private class Adapter : IPorts
        {
            public Adapter(GetCurrentDate getCurrentDate)
            {
                GetCurrentDate = getCurrentDate;
            }

            public GetCurrentDate GetCurrentDate { get; private set; }
            public IAuth Auth { get; }
            public IStorage Storage { get; set; }
        }
    }
}