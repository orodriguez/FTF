using FTF.Api;
using FTF.Core;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.Ports;
using FTF.Storage.EntityFramework;

namespace FTF.IoC.SimpleInjector
{
    public class ApplicationFactory
    {
        public IApplication Make(GetCurrentDate getCurrentDate) => 
            ContainerFactory.Make(new TestsPortsConfig(getCurrentDate))
                .GetInstance<IApplication>();
    }

    public class TestsPortsConfig : IPorts
    {
        public GetCurrentDate GetCurrentDate { get; }

        public IAuthPort AuthPort { get; }

        public IStoragePort StoragePort { get; set; }

        public TestsPortsConfig(GetCurrentDate getCurrentDate)
        {
            GetCurrentDate = getCurrentDate;
            StoragePort = new StorageAdapter("FTF.Tests");
            AuthPort = new FakeAuthAdapter();
        }
    }

    public class FakeAuthAdapter : IAuthPort
    {
        public User CurrentUser { get; set; }
    }
}