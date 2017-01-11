using System;
using System.Linq;
using FTF.Core.Delegates;

namespace FTF.Core
{
    public interface IStoragePort : IDisposable
    {
        int SaveChanges();

        IQueryable GetQueriable(Type entityType);

        Save<TEntity> MakeSave<TEntity>() where TEntity : class;
    }
}