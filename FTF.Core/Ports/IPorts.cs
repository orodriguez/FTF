using FTF.Core.Delegates;

namespace FTF.Core.Ports
{
    public interface IPorts
    {
        GetCurrentDate GetCurrentDate { get; }

        IAuthPort AuthPort { get; }

        IStoragePort Storage { get; }
    }
}