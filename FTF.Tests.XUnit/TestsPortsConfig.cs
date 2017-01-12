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

        public IAuth Auth { get; }

        public IStorage Storage { get; }

        public TestsPortsConfig(GetCurrentDate getCurrentDate)
        {
            GetCurrentDate = getCurrentDate;
            Auth = new FakeAuthAdapter();
            Storage = new RollbackStorage();
        }

        private class FakeAuthAdapter : IAuth
        {
            public User CurrentUser { get; set; }
        }

        private class RollbackStorage : IStorage
        {
            public DbContext Db { get; }

            private readonly DbContextTransaction _trans;

            public RollbackStorage()
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