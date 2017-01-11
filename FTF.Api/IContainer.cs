namespace FTF.Api
{
    public interface IContainer
    {
        T GetInstance<T>() where T : class;
    }
}