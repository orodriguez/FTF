using System.Linq;

namespace FTF.Core.Storage
{
    public interface IRepository<T> : IQueryable<T>
    {
        void Add(T entity);
    }
}