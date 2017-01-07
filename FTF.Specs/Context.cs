using System;
using System.Data.Entity;
using System.Linq;
using FTF.Api.Actions.Auth;
using FTF.Api.Actions.Notes;
using FTF.Api.Actions.Tags;
using FTF.Core.Auth.SignUp;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.Notes;
using SimpleInjector;
using DbContext = FTF.Storage.EntityFramework.DbContext;
using FTF.Core.Extensions.Queriable;
using SimpleInjector.Extensions.LifetimeScoping;
using Create = FTF.Api.Actions.Notes.Create;

namespace FTF.Specs
{
    public class Context : IDisposable
    {
        private User _currentUser;

        public DbContext Db => _container.GetInstance<DbContext>();

        public GetCurrentDate GetCurrentDate { get; set; }

        public DbContextTransaction Transaction { get; set; }

        public Exception Exception { get; private set; }

        public User CurrentUser
        {
            get
            {
                if (_currentUser == null)
                    throw new Exception("No user is logged in");

                return _currentUser;
            }
            set { _currentUser = value; }
        }

        private readonly Container _container;

        private Scope _scope;

        public GenerateNoteId NextId { get; set; }

        public Context()
        {
            GetCurrentDate = () => DateTime.Now;
            NextId = () => _container.GetInstance<IQueryable<Note>>().NextId();

            _container = RegisterTypes(new Container());

            Transaction = _container.GetInstance<DbContext>().Database.BeginTransaction();
        }

        private Container RegisterTypes(Container container)
        {
            container.Options.DefaultScopedLifestyle = new LifetimeScopeLifestyle();
            container.Register<Create>(() => container.GetInstance<CreateHandler>().Create);
            container.Register<CreateHandler>();
            container.Register(() => NextId);
            container.Register(() => GetCurrentDate);
            container.Register<Save<Note>>(() => container.GetInstance<DbContext>().Notes.Add);
            container.Register<SaveChanges>(() => container.GetInstance<DbContext>().SaveChanges);
            container.Register<IQueryable<Tag>>(() => container.GetInstance<DbContext>().Tags);
            container.Register<GetCurrentUser>(() => () => CurrentUser);
            container.Register<ValidateNote>(() => NoteValidator.Validate);
            container.Register<IQueryable<Note>>(() => container.GetInstance<DbContext>().Notes);
            container.Register(() => new DbContext("name=FTF.Tests", new DropCreateDatabaseAlways<DbContext>()),
               Lifestyle.Scoped);
            container.Register<Delete>(() => container.GetInstance<DeleteHandler>().Delete);
            container.Register<DeleteHandler>();
            container.Register<Retrieve>(() => container.GetInstance<Queries>().Retrieve);
            container.Register<Queries>();
            container.Register<GetCurrentUserId>(() => () => CurrentUser.Id);
            container.Register<SignUp>(() => container.GetInstance<Handler>().SignUp);
            container.Register<Handler>();
            container.Register<SignIn>(() => container.GetInstance<Core.Auth.SignIn.Handler>().SignIn);
            container.Register<Core.Auth.SignIn.Handler>();
            container.Register<Save<User>>(() => container.GetInstance<DbContext>().Users.Add);
            container.Register<IQueryable<User>>(() => container.GetInstance<DbContext>().Users);
            container.Register<SetCurrentUser>(() => user => CurrentUser = user);
            container.Register<ListAll>(() => container.GetInstance<Core.Tags.Queries>().ListAll);
            container.Register<Core.Tags.Queries>();
            container.Register<ListJoint>(() => container.GetInstance<Core.Tags.Queries>().ListJoint);
            container.Register<IQueryable<Tagging>>(() => container.GetInstance<DbContext>().Taggings);
            container.Register<Update>(() => container.GetInstance<UpdateHandler>().Update);
            container.Register<UpdateHandler>();

            _scope = container.BeginLifetimeScope();

            return container;
        }

        public void StoreException(Action func)
        {
            try
            {
                func();
            }
            catch (ApplicationException e)
            {
                Exception = e;
            }
        }

        public TReturn StoreExceptionAndReturn<TReturn>(Func<TReturn> func) where TReturn : class
        {
            TReturn result = null;
            try
            {
                result = func();
            }
            catch (ApplicationException e)
            {
                Exception = e;
            }
            return result;
        }

        public void Exec<T>(Action<T> action) where T : class =>
            StoreException(() => action(_container.GetInstance<T>()));

        public TReturn Query<T, TReturn>(Func<T, TReturn> func) where T : class where TReturn : class =>
            StoreExceptionAndReturn(() => func(_container.GetInstance<T>()));

        public void Dispose()
        {
            Transaction.Rollback();
            Db.Dispose();
            _scope.Dispose();
        }
    }
}