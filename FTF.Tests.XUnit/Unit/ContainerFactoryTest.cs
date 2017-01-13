using System;
using FTF.IoC.SimpleInjector;
using FTF.IoC.SimpleInjector.PortsConfig;
using SimpleInjector.Extensions.LifetimeScoping;
using Xunit;

namespace FTF.Tests.XUnit.Unit
{
    public class ContainerFactoryTest
    {
        [Fact]
        public void TestContainer()
        {
            var container = ContainerFactory.Make(
                new TestsPortsConfig(() => DateTime.Now), 
                new LifetimeScopeLifestyle());

            container.Verify();
        }
    }
}
