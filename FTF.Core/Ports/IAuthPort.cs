using FTF.Core.Entities;

namespace FTF.Core.Ports
{
    public interface IAuthPort
    {
        User CurrentUser { get; set; }
    }
}