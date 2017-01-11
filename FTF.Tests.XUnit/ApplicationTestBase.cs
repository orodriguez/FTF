using System;
using FTF.Api;
using FTF.Api.Services;
using FTF.IoC.SimpleInjector;

namespace FTF.Tests.XUnit
{
    public class ApplicationTestBase : IDisposable
    {
        protected IApplication App;

        public ApplicationTestBase()
        {
            App = new ApplicationFactory()
                .Make(getCurrentDate: () => new DateTime(2016, 2, 20));
        }

        public void Dispose() => App.Dispose();
    }
}