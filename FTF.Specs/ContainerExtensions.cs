using System;
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

            var concreteTypes = allTypes
                .Where(t => t.GetCustomAttributes<ConcreteAttribute>().Any())
                .ToArray();

            foreach (var type in concreteTypes)
                c.Register(type);

            var delegateMethods = allTypes
                .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
                .Where(method => method.GetCustomAttributes<Delegate>(inherit: false).Any())
                .ToArray();

            foreach (var method in delegateMethods)
            {
                var delegateType = method
                    .GetCustomAttribute<Delegate>()
                    .DelegateType;

                c.Register(delegateType, () => CreateInstance(method, delegateType, c.GetInstance(method.DeclaringType)));
            }

            var entities = allTypes
                .Where(t => t.GetInterfaces().Any(i => i == typeof (IEntity)));

            foreach (var entityType in entities)
            {
                var type = typeof(Save<>).MakeGenericType(entityType);

                c.Register(type, () =>
                {
                    var db = c.GetInstance<DbContext>();

                    var setMethod = db.GetType()
                        .GetMethod("Set", new Type[0])
                        .MakeGenericMethod(entityType);

                    var dbSet = setMethod.Invoke(db, null);

                    var addMethod = dbSet.GetType().GetMethod("Add");

                    return System.Delegate.CreateDelegate(type, dbSet, addMethod);
                });
            }

            // Storage.EntityFramework
            c.Register(() => new DbContext("name=FTF.Tests", new System.Data.Entity.DropCreateDatabaseAlways<DbContext>()),
                Lifestyle.Scoped);

            c.Register<SaveChanges>(() => c.GetInstance<DbContext>().SaveChanges);

            // Queriables
            c.Register(typeof(IQueryable<>), typeof(DbSetAdapter<>));
        }

        private static object CreateInstance(MethodInfo method, Type delegateType, object instance)
        {
            return method.IsStatic
                ? System.Delegate.CreateDelegate(delegateType, method)
                : System.Delegate.CreateDelegate(delegateType, instance, method);
        }
    }
}