using FTF.Core.Delegates;

namespace FTF.Core.Ports
{
    public interface IPorts
    {
        GetCurrentDate GetCurrentDate { get; }

        IAuth Auth { get; }

        IStorage Storage { get; }
    }
}