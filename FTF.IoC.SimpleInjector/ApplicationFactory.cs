using FTF.Api.Services;
using FTF.Core.Delegates;
using FTF.IoC.SimpleInjector.PortsConfig;
using SimpleInjector.Extensions.LifetimeScoping;

namespace FTF.IoC.SimpleInjector
{
    public class ApplicationFactory
    {
        public IApplication MakeTestApplication(
            GetCurrentTime getCurrentTime)
        {
            var container = ContainerFactory.Make(new TestsPortsConfig(getCurrentTime), 
                    new LifetimeScopeLifestyle());

            return new Application(container);
        }
    }
}