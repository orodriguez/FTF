using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.Notes;
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

            var concreteTypes = allTypes
                .Where(t => t.GetCustomAttributes<ConcreteAttribute>().Any())
                .ToArray();

            var delegateMethods = allTypes
                .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
                .Where(method => method.GetCustomAttributes<Delegate>(inherit: false).Any())
                .ToArray();

            foreach (var type in concreteTypes)
                c.Register(type);

            foreach (var method in delegateMethods)
            {
                var delegateType = method
                    .GetCustomAttribute<Delegate>()
                    .DelegateType;

                c.Register(
                    delegateType,
                    instanceCreator: () =>
                    {
                        var instance = c.GetInstance(method.DeclaringType);

                        if (method.IsStatic)
                            return System.Delegate.CreateDelegate(delegateType, method);

                        return System.Delegate.CreateDelegate(delegateType, instance, method);
                    });
            }

            // Storage.EntityFramework
            c.Register(() => new DbContext("name=FTF.Tests", new System.Data.Entity.DropCreateDatabaseAlways<DbContext>()),
                Lifestyle.Scoped);
            c.Register<SaveChanges>(() => c.GetInstance<DbContext>().SaveChanges);

            // Save<TEntity> delegates
            c.RegisterSaveDelegates();

            // Queriables
            c.Register(typeof(IQueryable<>), typeof(DbSetAdapter<>));
        }

        private static void RegisterSaveDelegates(this Container c)
        {
            c.ResolveUnregisteredType += (sender, e) =>
            {
                var type = e.UnregisteredServiceType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Save<>))
                {
                    var db = c.GetInstance<DbContext>();

                    var setMethod = db.GetType()
                        .GetMethod("Set", new Type[0])
                        .MakeGenericMethod(type.GetGenericArguments());

                    var dbSet = setMethod.Invoke(db, null);

                    var addMethod = dbSet.GetType().GetMethod("Add");

                    e.Register(() => System.Delegate.CreateDelegate(type, dbSet, addMethod));
                }
            };
        }
    }
}