using System.Linq;
using System.Reflection;
using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.Ports;
using FTF.IoC.SimpleInjector.PortsConfig;
using SimpleInjector;

namespace FTF.IoC.SimpleInjector
{
    public class ContainerFactory
    {
        public static Container Make(IPorts ports, ScopedLifestyle scopedLifestyle)
        {
            var c = new Container();

            c.Options.DefaultScopedLifestyle = scopedLifestyle;

            c.Register(() => ports.GetCurrentTime);

            c.Register<GetCurrentUser>(() => () => ports.Auth.CurrentUser);

            c.Register<SetCurrentUser>(() => user => ports.Auth.CurrentUser = user);

            c.Register(ports.Storage.MakeDbContext, Lifestyle.Scoped);

            var allTypes = typeof (Note).Assembly.GetExportedTypes();

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

        public static Container MakeWebApi(ScopedLifestyle scopedLifestyle) => 
            Make(new WebApiPorts(), scopedLifestyle);
    }
}