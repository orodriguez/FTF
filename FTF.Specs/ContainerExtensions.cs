using System;
using System.Linq;
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

namespace FTF.Specs
{
    static internal class ContainerExtensions
    {
        public static void RegisterTypes(this Container c, GenerateNoteId generateNoteId, GetCurrentDate getCurrentDate, GetCurrentUser getCurrentUser, GetCurrentUserId getCurrentUserId, SetCurrentUser setCurrentUser)
        {
            c.Options.DefaultScopedLifestyle = new LifetimeScopeLifestyle();
            c.Register<Create>(() => c.GetInstance<CreateHandler>().Create);
            c.Register<CreateHandler>();
            c.Register(() => generateNoteId);
            c.Register(() => getCurrentDate);
            c.Register<Save<Note>>(() => c.GetInstance<DbContext>().Notes.Add);
            c.Register<SaveChanges>(() => c.GetInstance<DbContext>().SaveChanges);
            c.Register<IQueryable<Tag>>(() => c.GetInstance<DbContext>().Tags);
            c.Register(() => getCurrentUser);
            c.Register<ValidateNote>(() => NoteValidator.Validate);
            c.Register<IQueryable<Note>>(() => c.GetInstance<DbContext>().Notes);
            c.Register(() => new DbContext("name=FTF.Tests", new System.Data.Entity.DropCreateDatabaseAlways<DbContext>()),
                Lifestyle.Scoped);
            c.Register<Delete>(() => c.GetInstance<DeleteHandler>().Delete);
            c.Register<DeleteHandler>();
            c.Register<Retrieve>(() => c.GetInstance<Queries>().Retrieve);
            c.Register<Queries>();
            c.Register(() => getCurrentUserId);
            c.Register<SignUp>(() => c.GetInstance<Handler>().SignUp);
            c.Register<Handler>();
            c.Register<SignIn>(() => c.GetInstance<FTF.Core.Auth.SignIn.Handler>().SignIn);
            c.Register<FTF.Core.Auth.SignIn.Handler>();
            c.Register<Save<User>>(() => c.GetInstance<DbContext>().Users.Add);
            c.Register<IQueryable<User>>(() => c.GetInstance<DbContext>().Users);
            c.Register<SetCurrentUser>(() => setCurrentUser);
            c.Register<ListAll>(() => c.GetInstance<FTF.Core.Tags.Queries>().ListAll);
            c.Register<FTF.Core.Tags.Queries>();
            c.Register<ListJoint>(() => c.GetInstance<FTF.Core.Tags.Queries>().ListJoint);
            c.Register<IQueryable<Tagging>>(() => c.GetInstance<DbContext>().Taggings);
            c.Register<Update>(() => c.GetInstance<UpdateHandler>().Update);
            c.Register<UpdateHandler>();
        }
    }
}