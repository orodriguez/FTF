using System.Data.Entity;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.Ports;
using DbContext = FTF.Core.EntityFramework.DbContext;

namespace FTF.IoC.SimpleInjector.PortsConfig
{
    public class TestsPortsConfig : IPorts
    {
        public GetCurrentTime GetCurrentTime { get; }

        public IAuth Auth { get; }

        public IStorage Storage { get; }

        public TestsPortsConfig(GetCurrentTime getCurrentTime)
        {
            GetCurrentTime = getCurrentTime;
            Auth = new FakeAuthAdapter();
            Storage = new RollbackStorage();
        }

        private class FakeAuthAdapter : IAuth
        {
            public User CurrentUser { get; set; }
        }

        private class RollbackStorage : IStorage
        {
            public DbContext MakeDbContext() => 
                new DbContext("name=FTF.Tests", new DropCreateDatabaseIfModelChanges<DbContext>());
        }
    }
}