using FTF.Api;
using FTF.Core.Ports;

namespace FTF.IoC.SimpleInjector
{
    public class ApplicationFactory
    {
        public IApplication Make(IPorts ports) => ContainerFactory.Make(ports).GetInstance<IApplication>();
    }
}