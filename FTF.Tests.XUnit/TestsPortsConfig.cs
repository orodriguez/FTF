using System.Data.Entity;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.Ports;
using DbContext = FTF.Core.EntityFramework.DbContext;

namespace FTF.Tests.XUnit
{
    public class TestsPortsConfig : IPorts
    {
        public GetCurrentDate GetCurrentDate { get; }

        public IAuthPort AuthPort { get; }

        public IStoragePort Storage { get; }

        public TestsPortsConfig(GetCurrentDate getCurrentDate)
        {
            GetCurrentDate = getCurrentDate;
            AuthPort = new FakeAuthAdapter();
            Storage = new StoragePort();
        }

        private class FakeAuthAdapter : IAuthPort
        {
            public User CurrentUser { get; set; }
        }

        private class StoragePort : IStoragePort
        {
            public DbContext Db { get; }

            private readonly DbContextTransaction _trans;

            public StoragePort()
            {
                Db = new DbContext("name=FTF.Tests", 
                    new DropCreateDatabaseIfModelChanges<DbContext>());

                _trans = Db.Database.BeginTransaction();
            }

            public void Dispose()
            {
                _trans.Rollback();
                Db.Dispose();
            }
        }
    }
}