using System.Data.Entity;
using FTF.Core;

namespace FTF.Storage.EntityFramework
{
    public class DbContext : System.Data.Entity.DbContext
    {
        public DbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<DbContext>());
        }

        public IDbSet<Note> Notes { get; set; }
    }
}