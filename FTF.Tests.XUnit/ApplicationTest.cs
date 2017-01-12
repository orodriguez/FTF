using System;
using FTF.Api;
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

            App = new ApplicationFactory()
                .Make(getCurrentDate: () => CurrentTime);
        }

        public void Dispose() => App.Dispose();
    }
}