using System.Data.Entity;
using FTF.Core;
using FTF.Core.Storage;

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

        public void Dispose() => _transaction.Rollback();

        public IUnitOfWork MakeUnitOfWork() => new UnitOfWork(_db);

        public IRepository<T> MakeRepository<T>() where T : class => new Repository<T>(_db.Set<T>());
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _db;

        public UnitOfWork(DbContext db)
        {
            _db = db;
        }

        public int SaveChanges() => _db.SaveChanges();
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IDbSet<T> _set;

        public Repository(IDbSet<T> set)
        {
            _set = set;
        }

        public void Add(T entity)
        {
            _set.Add(entity);
        }
    }
}