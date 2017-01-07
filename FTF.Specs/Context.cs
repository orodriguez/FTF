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

        private readonly Scope _scope;

        public GenerateNoteId NextId { get; set; }

        public Context()
        {
            GetCurrentDate = () => DateTime.Now;
            NextId = () => _container.GetInstance<IQueryable<Note>>().NextId();
            _container = new Container();
            _container.Options.DefaultScopedLifestyle = new LifetimeScopeLifestyle();
            _container.Register<Create>(() => _container.GetInstance<CreateHandler>().Create);
            _container.Register<CreateHandler>();
            _container.Register(() => NextId);
            _container.Register(() => GetCurrentDate);
            _container.Register<Save<Note>>(() => _container.GetInstance<DbContext>().Notes.Add);
            _container.Register<SaveChanges>(() => _container.GetInstance<DbContext>().SaveChanges);
            _container.Register<IQueryable<Tag>>(() => _container.GetInstance<DbContext>().Tags);
            _container.Register<GetCurrentUser>(() => () => CurrentUser);
            _container.Register<ValidateNote>(() => NoteValidator.Validate);
            _container.Register<IQueryable<Note>>(() => _container.GetInstance<DbContext>().Notes);
            _container.Register(() => new DbContext("name=FTF.Tests", new DropCreateDatabaseAlways<DbContext>()), Lifestyle.Scoped);
            _container.Register<Delete>(() => _container.GetInstance<DeleteHandler>().Delete);
            _container.Register<DeleteHandler>();
            _container.Register<Retrieve>(() => _container.GetInstance<Queries>().Retrieve);
            _container.Register<Queries>();
            _container.Register<GetCurrentUserId>(() => () => CurrentUser.Id);
            _container.Register<SignUp>(() => _container.GetInstance<Handler>().SignUp);
            _container.Register<Handler>();
            _container.Register<SignIn>(() => _container.GetInstance<Core.Auth.SignIn.Handler>().SignIn);
            _container.Register<Core.Auth.SignIn.Handler>();
            _container.Register<Save<User>>(() => _container.GetInstance<DbContext>().Users.Add);
            _container.Register<IQueryable<User>>(() => _container.GetInstance<DbContext>().Users);
            _container.Register<SetCurrentUser>(() => user => CurrentUser = user);
            _container.Register<ListAll>(() => _container.GetInstance<Core.Tags.Queries>().ListAll);
            _container.Register<Core.Tags.Queries>();
            _container.Register<ListJoint>(() => _container.GetInstance<Core.Tags.Queries>().ListJoint);
            _container.Register<IQueryable<Tagging>>(() => _container.GetInstance<DbContext>().Taggings);
            _container.Register<Update>(() => _container.GetInstance<UpdateHandler>().Update);
            _container.Register<UpdateHandler>();

            _scope = _container.BeginLifetimeScope();
            Transaction = _container.GetInstance<DbContext>().Database.BeginTransaction();
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
                result =  func();
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