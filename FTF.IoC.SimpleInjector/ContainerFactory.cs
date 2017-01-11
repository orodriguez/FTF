using FTF.Api;
using SimpleInjector;

namespace FTF.IoC.SimpleInjector
{
    public class ContainerFactory
    {
        public static IContainer Make() => 
            new Adapter(new Container());

        private class Adapter : IContainer
        {
            private readonly Container _container;

            internal Adapter(Container container)
            {
                _container = container;
            }

            public T GetInstance<T>() where T : class => _container.GetInstance<T>();
        }
    }
}