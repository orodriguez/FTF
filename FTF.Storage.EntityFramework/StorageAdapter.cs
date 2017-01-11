using System;
using System.Data.Entity;
using System.Linq;
using FTF.Core.Delegates;
using FTF.Core.Ports;

namespace FTF.Storage.EntityFramework
{
    public class StorageAdapter : IStorage
    {
        private readonly DbContext _db;

        public StorageAdapter(string nameOrConnectionString)
        {
            _db = new DbContext(nameOrConnectionString, 
                new DropCreateDatabaseIfModelChanges<DbContext>());
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public IQueryable GetQueriable(Type entityType)
        {
            throw new NotImplementedException();
        }

        public Save<TEntity> MakeSave<TEntity>() where TEntity : class => _db.Set<TEntity>().Add;
    }
}