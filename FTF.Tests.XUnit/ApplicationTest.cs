using System;
using FTF.Api.Services;
using FTF.IoC.SimpleInjector;

namespace FTF.Tests.XUnit
{
    public class ApplicationTest : IDisposable
    {
        protected IApplication App;

        protected DateTime CurrentTime;

        public ApplicationTest()
        {
            CurrentTime = DateTime.Now;

            App = new ApplicationFactory().MakeTestApplication(() => CurrentTime);
        }

        public void Dispose() => App.Dispose();
    }
}