using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using FTF.Api.Actions.Auth;
using FTF.Api.Actions.Notes;
using FTF.Api.Actions.Tags;
using FTF.Core.Auth.SignUp;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.Notes;
using FTF.Storage.EntityFramework;
using SimpleInjector;
using SimpleInjector.Extensions.LifetimeScoping;
using Create = FTF.Api.Actions.Notes.Create;
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

            // Auth
            c.Register<SignUp>(() => c.GetInstance<Handler>().SignUp);
            c.Register<SignIn>(() => c.GetInstance<FTF.Core.Auth.SignIn.Handler>().SignIn);

            // Notes
            c.Register<Create>(() => c.GetInstance<CreateHandler>().Create);
            c.Register<Retrieve>(() => c.GetInstance<Queries>().Retrieve);
            c.Register<Update>(() => c.GetInstance<UpdateHandler>().Update);
            c.Register<Delete>(() => c.GetInstance<DeleteHandler>().Delete);

            // Tags
            c.Register<ListAll>(() => c.GetInstance<Core.Tags.Queries>().ListAll);
            c.Register<ListJoint>(() => c.GetInstance<FTF.Core.Tags.Queries>().ListJoint);

            // Actions implementations
            c.Register<Handler>();
            c.Register<Core.Auth.SignIn.Handler>();
            c.Register<CreateHandler>();
            c.Register<UpdateHandler>();
            c.Register<DeleteHandler>();
            c.Register<Queries>();
            c.Register<Core.Tags.Queries>();

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

                if (type.GetGenericTypeDefinition() == typeof (Save<>))
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