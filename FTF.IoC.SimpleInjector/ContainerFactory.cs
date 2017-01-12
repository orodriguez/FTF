using System.Linq;
using System.Reflection;
using FTF.Core;
using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.Ports;
using SimpleInjector;

namespace FTF.IoC.SimpleInjector
{
    public class ContainerFactory
    {
        public static Container Make(IPorts ports)
        {
            var c = new Container();

            c.Register(() => ports.Storage);

            c.Register(() => ports.GetCurrentDate);

            c.Register<GetCurrentUser>(() => () => ports.AuthPort.CurrentUser);

            c.Register<SetCurrentUser>(() => user => ports.AuthPort.CurrentUser = user);

            c.Register(() => ports.Storage.Db);

            var allTypes = typeof (Application).Assembly.GetExportedTypes();

            allTypes
                .Where(t => t.GetCustomAttributes<ConcreteAttribute>().Any())
                .ToList()
                .ForEach(type => c.Register(type));

            allTypes
                .Where(t => t.GetCustomAttributes<RoleAttribute>().Any())
                .Select(t => new
                {
                    ServiceType =  t.GetCustomAttribute<RoleAttribute>().RoleType,
                    ImplementationType = t
                })
                .ToList()
                .ForEach(obj => c.Register(obj.ServiceType, obj.ImplementationType));

            return c;
        }
    }
}