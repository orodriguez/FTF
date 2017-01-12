using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FTF.Core;
using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.Ports;
using FTF.Storage.EntityFramework;
using SimpleInjector;
using SimpleInjector.Extensions.LifetimeScoping;

namespace FTF.IoC.SimpleInjector
{
    public class ContainerFactory
    {
        public static Container Make(IPorts ports)
        {
            var c = new Container();

            c.Options.DefaultScopedLifestyle = new LifetimeScopeLifestyle();

            c.Register(() => ports.StoragePort);

            c.Register(() => ports.GetCurrentDate);

            c.Register<GetCurrentUser>(() => () => ports.AuthPort.CurrentUser);

            c.Register<SetCurrentUser>(() => user => ports.AuthPort.CurrentUser = user);

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

            RegisterDelegates(c, allTypes);

            var entities = allTypes
                .Where(t => t.GetInterfaces().Any(i => i == typeof(IEntity)));

            foreach (var entityType in entities)
            {
                var makeSave = ports.StoragePort
                    .GetType()
                    .GetMethod("MakeSave")
                    .MakeGenericMethod(entityType);

                c.Register(typeof(Save<>).MakeGenericType(entityType), 
                    () => makeSave.Invoke(ports.StoragePort, null));

                c.Register(typeof (IQueryable<>).MakeGenericType(entityType), 
                    () => ports.StoragePort.GetQueriable(entityType));
            }

            c.Register<SaveChanges>(() => ports.StoragePort.SaveChanges);

            return c;
        }

        private static void RegisterDelegates(Container c, Type[] allTypes)
        {
            const BindingFlags bindingFlags = BindingFlags.Public
                | BindingFlags.Instance
                | BindingFlags.Static
                | BindingFlags.DeclaredOnly;

            var delegateMethods = allTypes
                .SelectMany(t => t.GetMethods(bindingFlags))
                .Where(method => method.GetCustomAttributes<RoleAttribute>(inherit: false).Any())
                .ToArray();

            foreach (var method in delegateMethods)
            {
                var delegateType = method
                    .GetCustomAttribute<RoleAttribute>()
                    .RoleType;

                c.Register(delegateType, () => CreateDelegate(c, method, delegateType));
            }
        }

        private static object CreateDelegate(Container c, MethodInfo method, Type delegateType)
        {
            if (method.IsStatic)
                return System.Delegate.CreateDelegate(delegateType, method);

            return System.Delegate.CreateDelegate(
                delegateType,
                c.GetInstance(method.DeclaringType),
                method);
        }
    }
}