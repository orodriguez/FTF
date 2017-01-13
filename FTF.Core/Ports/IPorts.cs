using FTF.Core.Delegates;

namespace FTF.Core.Ports
{
    public interface IPorts
    {
        GetCurrentTime GetCurrentTime { get; }

        IAuth Auth { get; }

        IStorage Storage { get; }
    }
}