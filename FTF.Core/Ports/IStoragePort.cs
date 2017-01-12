using System;
using FTF.Core.EntityFramework;

namespace FTF.Core.Ports
{
    public interface IStoragePort : IDisposable
    {
        DbContext Db { get; }
    }
}