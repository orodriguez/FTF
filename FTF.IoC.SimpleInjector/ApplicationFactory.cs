using FTF.Api.Services;
using FTF.Core.Delegates;
using FTF.IoC.SimpleInjector.PortsConfig;

namespace FTF.IoC.SimpleInjector
{
    public class ApplicationFactory
    {
        public IApplication MakeTestApplication(GetCurrentTime getCurrentTime) => 
            ContainerFactory.Make(new TestsPortsConfig(getCurrentTime))
                .GetInstance<IApplication>();
    }
}