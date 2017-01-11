using System;
using FTF.Api;
using FTF.Api.Actions.Auth;
using FTF.Api.Responses;
using FTF.Core.Delegates;
using FTF.Core.Ports;
using FTF.IoC.SimpleInjector;
using FTF.Storage.EntityFramework;

namespace FTF.Tests.XUnit
{
    public class ApiTest : IDisposable
    {
        private DateTime _currentDate;

        private readonly IContainer _container;

        public ApiTest()
        {
            _container = ContainerFactory.Make(new Adapter(
                getCurrentDate: () => _currentDate, 
                storage: new StorageAdapter("FTF.Test")));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        protected void SignIn(string username)
        {
            var signIn = _container.GetInstance<SignIn>();
            signIn(username);
        }

        protected void SignUp(string username)
        {
            var signUp = _container.GetInstance<SignUp>();
            signUp(username);
        }

        protected void TodayIs(int year, int month, int day) => 
            _currentDate = new DateTime(year, month, day);

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
            public Adapter(GetCurrentDate getCurrentDate, IStorage storage)
            {
                GetCurrentDate = getCurrentDate;
                Storage = storage;
            }

            public GetCurrentDate GetCurrentDate { get; private set; }
            public IAuth Auth { get; }
            public IStorage Storage { get; set; }
        }
    }
}