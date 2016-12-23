using FTF.Core;
using FTF.Storage.EntityFramework;
using TechTalk.SpecFlow;

namespace FTF.Specs
{
    [Binding]
    public class Hooks
    {
        private Context _context;

        public Hooks(Context context)
        {
            _context = context;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _context.Db = new DbContext("name=FTF.Tests", new System.Data.Entity.DropCreateDatabaseAlways<DbContext>());
            _context.Transaction = _context.Db.Database.BeginTransaction();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _context.Transaction.Rollback();
            _context.Db.Dispose();
            _context = null;
        }
    }
}