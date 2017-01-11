using System;
using System.Linq;
using FTF.Core.Delegates;
using FTF.Core.Entities;

namespace FTF.Core.Ports
{
    public interface IPorts
    {
        GetCurrentDate GetCurrentDate { get; }

        IAuth Auth { get; }

        IStorage Storage { get; set; }
    }

    public interface IStorage
    {
        int SaveChanges();

        IQueryable GetQueriable(Type entityType);

        Save<TEntity> MakeSave<TEntity>() where TEntity : class;
    }

    public interface IAuth
    {
        User CurrentUser { get; set; }
    }
}