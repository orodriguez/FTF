using System;
using FTF.Core.Storage;

namespace FTF.Core
{
    public interface IStoragePort : IDisposable
    {
        IRepository<T> MakeRepository<T>() where T : class;
        IUnitOfWork MakeUnitOfWork();
    }
}