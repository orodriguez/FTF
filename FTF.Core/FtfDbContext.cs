using System.Data.Entity;

namespace FTF.Core
{
    public class FtfDbContext : DbContext
    {
        public FtfDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<FtfDbContext>());
        }

        public IDbSet<Note> Notes { get; set; }
    }
}