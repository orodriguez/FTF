using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FTF.Core;
using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Storage.EntityFramework;
using SimpleInjector;
using SimpleInjector.Extensions.LifetimeScoping;
using DbContext = FTF.Storage.EntityFramework.DbContext;
using Delegate = FTF.Core.Attributes.Delegate;

namespace FTF.Specs
{
    static internal class ContainerExtensions
    {
        // TODO: Refactor: Too many parameters
        public static void RegisterTypes(this Container c, 
            GenerateNoteId generateNoteId, 
            GetCurrentDate getCurrentDate, 
            GetCurrentUser getCurrentUser, 
            GetCurrentUserId getCurrentUserId, 
            SetCurrentUser setCurrentUser)
        {
            c.Options.DefaultScopedLifestyle = new LifetimeScopeLifestyle();

            // Tests Specific Types
            c.Register(() => generateNoteId);
            c.Register(() => getCurrentDate);
            c.Register(() => getCurrentUser);
            c.Register(() => getCurrentUserId);
            c.Register(() => setCurrentUser);

            var assemblies = new[]
            {
                typeof (Note).Assembly,
                typeof (DbContext).Assembly
            };

            var allTypes = assemblies
                .SelectMany(a => a.GetExportedTypes())
                .ToArray();

            c.RegisterConcreteTypes(allTypes);

            c.RegisterDelegates(allTypes);

            var entities = allTypes
                .Where(t => t.GetInterfaces().Any(i => i == typeof (IEntity)));

            foreach (var entityType in entities)
                c.RegisterSaveDelagates(entityType);

            // Storage.EntityFramework
            c.Register(() => new DbContext("name=FTF.Tests", new System.Data.Entity.DropCreateDatabaseAlways<DbContext>()),
                Lifestyle.Scoped);

            c.Register<SaveChanges>(() => c.GetInstance<DbContext>().SaveChanges);

            // Queriables
            c.Register(typeof(IQueryable<>), typeof(DbSetAdapter<>));
        }

        private static void RegisterDelegates(this Container c, Type[] allTypes)
        {
            const BindingFlags bindingFlags = BindingFlags.Public 
                | BindingFlags.Instance 
                | BindingFlags.Static 
                | BindingFlags.DeclaredOnly;

            var delegateMethods = allTypes
                .SelectMany(t => t.GetMethods(bindingFlags))
                .Where(method => method.GetCustomAttributes<Delegate>(inherit: false).Any())
                .ToArray();

            foreach (var method in delegateMethods)
            {
                var delegateType = method
                    .GetCustomAttribute<Delegate>()
                    .DelegateType;

                c.Register(delegateType, () => CreateDelegate(c, method, delegateType));
            }
        }

        private static void RegisterConcreteTypes(this Container c, IEnumerable<Type> allTypes)
        {
            var concreteTypes = allTypes
                .Where(t => t.GetCustomAttributes<ConcreteAttribute>().Any())
                .ToArray();

            foreach (var type in concreteTypes)
                c.Register(type);
        }

        private static void RegisterSaveDelagates(this Container c, Type entityType)
        {
            var saveType = typeof (Save<>).MakeGenericType(entityType);

            c.Register(saveType, () =>
            {
                var db = c.GetInstance<DbContext>();

                var setMethod = db.GetType()
                    .GetMethod("Set", new Type[0])
                    .MakeGenericMethod(entityType);

                var dbSet = setMethod.Invoke(db, null);

                var addMethod = dbSet.GetType().GetMethod("Add");

                return System.Delegate.CreateDelegate(saveType, dbSet, addMethod);
            });
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