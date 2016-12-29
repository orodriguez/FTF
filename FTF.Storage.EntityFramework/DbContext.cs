using System.Data.Entity;
using FTF.Core.Entities;

namespace FTF.Storage.EntityFramework
{
    public class DbContext : System.Data.Entity.DbContext
    {
        public DbContext() : this("FTF",
            initializer: new CreateDatabaseIfNotExists<DbContext>())
        {
        }

        public DbContext(string nameOrConnectionString, IDatabaseInitializer<DbContext> initializer) 
            : base(nameOrConnectionString)
        {
            Database.SetInitializer(initializer);
        }

        public IDbSet<Note> Notes { get; set; }

        public IDbSet<Tag> Tags { get; set; }

        public IDbSet<User> Users { get; set; }
    }
}