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
    }

    public interface IAuth
    {
        User CurrentUser { get; set; }
    }
}