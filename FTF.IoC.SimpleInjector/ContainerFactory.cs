﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FTF.Api;
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
        public static IContainer Make(IPorts ports) => 
            new Adapter(RegisterTypes(ports));

        public static Container RegisterTypes(IPorts ports)
        {
            var c = new Container();

            c.Options.DefaultScopedLifestyle = new LifetimeScopeLifestyle();

            c.Register(() => ports.GetCurrentDate);
            c.Register<GetCurrentUser>(() => () => ports.Auth.CurrentUser);
            c.Register<GetCurrentUserId>(() => () => ports.Auth.CurrentUser.Id);
            c.Register<SetCurrentUser>(() => user => ports.Auth.CurrentUser = user);

            var assemblies = new[]
            {
                typeof (Note).Assembly,
                typeof (DbContext).Assembly
            };

            var allTypes = assemblies
                .SelectMany(a => a.GetExportedTypes())
                .ToArray();

            RegisterConcreteTypes(c, allTypes);

            RegisterDelegates(c, allTypes);

            var entities = allTypes
                .Where(t => t.GetInterfaces().Any(i => i == typeof(IEntity)));

            foreach (var entityType in entities)
            {
                var makeSave = ports.Storage
                    .GetType()
                    .GetMethod("MakeSave")
                    .MakeGenericMethod(entityType);

                c.Register(typeof(Save<>).MakeGenericType(entityType), 
                    () => makeSave.Invoke(ports.Storage, null));

                c.Register(typeof (IQueryable<>).MakeGenericType(entityType), 
                    () => ports.Storage.GetQueriable(entityType));
            }

            c.Register<SaveChanges>(() => ports.Storage.SaveChanges);

            return c;
        }

        private static Delegate CreateSaveDelegate(IPorts ports, Type entityType)
        {
            var saveType = typeof (Save<>).MakeGenericType(entityType);

            var saveMethod = ports.Storage.GetType()
                .GetMethod("Save").MakeGenericMethod(entityType);

            var saveDel = Delegate.CreateDelegate(saveType, ports.Storage, saveMethod);

            return saveDel;
        }

        private static void RegisterDelegates(Container c, Type[] allTypes)
        {
            const BindingFlags bindingFlags = BindingFlags.Public
                | BindingFlags.Instance
                | BindingFlags.Static
                | BindingFlags.DeclaredOnly;

            var delegateMethods = allTypes
                .SelectMany(t => t.GetMethods(bindingFlags))
                .Where(method => method.GetCustomAttributes<Role>(inherit: false).Any())
                .ToArray();

            foreach (var method in delegateMethods)
            {
                var delegateType = method
                    .GetCustomAttribute<Role>()
                    .DelegateType;

                c.Register(delegateType, () => CreateDelegate(c, method, delegateType));
            }
        }

        private static void RegisterConcreteTypes(Container c, IEnumerable<Type> allTypes)
        {
            var concreteTypes = allTypes
                .Where(t => t.GetCustomAttributes<ConcreteAttribute>().Any())
                .ToArray();

            foreach (var type in concreteTypes)
                c.Register(type);
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