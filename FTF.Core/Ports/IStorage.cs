using System;
using FTF.Core.EntityFramework;

namespace FTF.Core.Ports
{
    public interface IStorage : IDisposable
    {
        DbContext Db { get; }
    }
}