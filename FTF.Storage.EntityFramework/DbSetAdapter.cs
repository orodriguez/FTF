using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace FTF.Storage.EntityFramework
{
    public class DbSetAdapter<T> : IDbSet<T> where T : class
    {
        private readonly IDbSet<T> _dbSet;

        public DbSetAdapter(DbContext context)
        {
            _dbSet = context.Set<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _dbSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Expression Expression => _dbSet.Expression;

        public Type ElementType => _dbSet.ElementType;

        public IQueryProvider Provider => _dbSet.Provider;

        public T Find(params object[] keyValues) => _dbSet.Find(keyValues);

        public T Add(T entity) => _dbSet.Add(entity);

        public T Remove(T entity) => _dbSet.Remove(entity);

        public T Attach(T entity) => _dbSet.Attach(entity);

        public T Create() => _dbSet.Create();

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T => _dbSet.Create<TDerivedEntity>();

        public ObservableCollection<T> Local => _dbSet.Local;
    }
}