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
            _context.Db = new DbContext("FTF.Tests");
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