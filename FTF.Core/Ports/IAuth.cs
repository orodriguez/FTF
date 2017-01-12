using FTF.Core.Entities;

namespace FTF.Core.Ports
{
    public interface IAuth
    {
        User CurrentUser { get; set; }
    }
}