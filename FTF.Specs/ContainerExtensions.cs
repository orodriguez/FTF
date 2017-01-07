using System;
using System.Linq;
using System.Reflection;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.Notes;
using FTF.Storage.EntityFramework;
using SimpleInjector;
using SimpleInjector.Extensions.LifetimeScoping;
using Action = FTF.Core.Attributes.Action;
using DbContext = FTF.Storage.EntityFramework.DbContext;

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

            // Actions delegates
            var methods = typeof(Note).Assembly
                .GetExportedTypes()
                .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                .Where(method => method.GetCustomAttributes<Action>(inherit: false).Any())
                .ToArray();

            var declaringTypes = methods.Select(m => m.DeclaringType).Distinct();

            foreach (var type in declaringTypes)
                c.Register(type);

            foreach (var method in methods)
            {
                var delegateType = method
                    .GetCustomAttribute<Action>()
                    .DelegateType;

                c.Register(
                    delegateType,
                    instanceCreator: () =>
                    {
                        var instance = c.GetInstance(method.DeclaringType);

                        return Delegate.CreateDelegate(delegateType, instance, method);
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

            // Validators
            c.Register<ValidateNote>(() => NoteValidator.Validate);
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

                    e.Register(() => Delegate.CreateDelegate(type, dbSet, addMethod));
                }
            };
        }
    }
}