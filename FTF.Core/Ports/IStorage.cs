using FTF.Core.EntityFramework;

namespace FTF.Core.Ports
{
    public interface IStorage
    {
        DbContext MakeDbContext();
    }
}