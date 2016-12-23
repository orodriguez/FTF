using System.Data.Entity;

namespace FTF.Specs
{
    public class FtfDbContext : DbContext
    {
        public FtfDbContext() : base("FTF.Tests")
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<FtfDbContext>());
        }

        public IDbSet<Note> Notes { get; set; }
    }
}