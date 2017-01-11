using System;
using System.Data.Entity;
using System.Linq;
using FTF.Core;
using FTF.Core.Delegates;

namespace FTF.Storage.EntityFramework
{
    public class StorageAdapter : IStoragePort
    {
        private readonly DbContext _db;

        private readonly DbContextTransaction _transaction;

        public StorageAdapter(string nameOrConnectionString)
        {
            _db = new DbContext(nameOrConnectionString, 
                new DropCreateDatabaseIfModelChanges<DbContext>());

            _transaction = _db.Database.BeginTransaction();
        }

        public int SaveChanges() => _db.SaveChanges();

        public IQueryable GetQueriable(Type entityType) => _db.Set(entityType);

        public Save<TEntity> MakeSave<TEntity>() where TEntity : class => _db.Set<TEntity>().Add;

        public void Dispose() => _transaction.Rollback();
    }
}