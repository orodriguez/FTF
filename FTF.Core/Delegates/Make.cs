namespace FTF.Core.Delegates
{
    public delegate T Make<out T>();

    public delegate TReturn Make<in TArg, out TReturn>(TArg arg);
}