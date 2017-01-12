namespace FTF.Core.Storage
{
    public interface IUnitOfWork
    {
        int SaveChanges();
    }
}