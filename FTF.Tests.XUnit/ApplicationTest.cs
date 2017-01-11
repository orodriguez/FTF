using System;
using FTF.Api;
using FTF.IoC.SimpleInjector;

namespace FTF.Tests.XUnit
{
    public class ApplicationTest : IDisposable
    {
        protected readonly IApplication App;

        public ApplicationTest()
        {
            App = new ApplicationFactory()
                .Make(getCurrentDate: () => new DateTime(2016, 2, 20));

            App.Auth.SignUp("orodriguez");
            App.Auth.SignIn("orodriguez");
        }

        public void Dispose() => App.Dispose();
    }
}