using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FTF.Core;
using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.Ports;
using FTF.Core.Storage;
using SimpleInjector;

namespace FTF.IoC.SimpleInjector
{
    public class ContainerFactory
    {
        public static Container Make(IPorts ports)
        {
            var c = new Container();

            c.Register(() => ports.StoragePort);

            c.Register(() => ports.GetCurrentDate);

            c.Register<GetCurrentUser>(() => () => ports.AuthPort.CurrentUser);

            c.Register<SetCurrentUser>(() => user => ports.AuthPort.CurrentUser = user);

            c.Register(() => ports.StoragePort.MakeUnitOfWork());

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

            allTypes
                .Where(t => t.GetInterfaces().Any(i => i == typeof(IEntity)))
                .ToList().ForEach(t =>
                {
                    c.Register(typeof(IRepository<>).MakeGenericType(t), () => MakeRepository(t, ports.StoragePort));
                    c.Register(typeof(IQueryable<>).MakeGenericType(t), () => MakeRepository(t, ports.StoragePort));
                });

            return c;
        }

        private static object MakeRepository(Type type, IStoragePort storage)
        {
            return storage
                .GetType()
                .GetMethod("MakeRepository")
                .MakeGenericMethod(type)
                .Invoke(storage, null);
        }
    }
}