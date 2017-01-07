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
using TechTalk.SpecFlow;
using Create = FTF.Api.Actions.Notes.Create;

namespace FTF.Specs
{
    public class Context : IDisposable
    {
        private User _currentUser;

        public DbContext Db => _container.GetInstance<DbContext>();

        public GetCurrentDate GetCurrentDate { get; set; }

        public DbContextTransaction Transaction { get; set; }

        public ApplicationException Exception { get; private set; }

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
            RegisterTypes(_container, 
                generateNoteId: () => NextId(), 
                getCurrentDate: () => GetCurrentDate(), 
                getCurrentUser: () => CurrentUser, 
                getCurrentUserId: () => CurrentUser.Id, 
                setCurrentUser: user => CurrentUser = user);

            _scope = _container.BeginLifetimeScope();

            Transaction = _container.GetInstance<DbContext>().Database.BeginTransaction();
        }

        private static void RegisterTypes(Container c, GenerateNoteId generateNoteId, GetCurrentDate getCurrentDate, GetCurrentUser getCurrentUser, GetCurrentUserId getCurrentUserId, SetCurrentUser setCurrentUser)
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
            c.Register(() => new DbContext("name=FTF.Tests", new DropCreateDatabaseAlways<DbContext>()),
               Lifestyle.Scoped);
            c.Register<Delete>(() => c.GetInstance<DeleteHandler>().Delete);
            c.Register<DeleteHandler>();
            c.Register<Retrieve>(() => c.GetInstance<Queries>().Retrieve);
            c.Register<Queries>();
            c.Register(() => getCurrentUserId);
            c.Register<SignUp>(() => c.GetInstance<Handler>().SignUp);
            c.Register<Handler>();
            c.Register<SignIn>(() => c.GetInstance<Core.Auth.SignIn.Handler>().SignIn);
            c.Register<Core.Auth.SignIn.Handler>();
            c.Register<Save<User>>(() => c.GetInstance<DbContext>().Users.Add);
            c.Register<IQueryable<User>>(() => c.GetInstance<DbContext>().Users);
            c.Register<SetCurrentUser>(() => setCurrentUser);
            c.Register<ListAll>(() => c.GetInstance<Core.Tags.Queries>().ListAll);
            c.Register<Core.Tags.Queries>();
            c.Register<ListJoint>(() => c.GetInstance<Core.Tags.Queries>().ListJoint);
            c.Register<IQueryable<Tagging>>(() => c.GetInstance<DbContext>().Taggings);
            c.Register<Update>(() => c.GetInstance<UpdateHandler>().Update);
            c.Register<UpdateHandler>();
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

        public void Exec<T>(Action<T> action) where T : class
        {
            var instance = _container.GetInstance<T>();

            if (ScenarioContext.Current.ScenarioInfo.Tags.Contains("error"))
                try
                {
                    action(instance);
                }
                catch (ApplicationException ae)
                {
                    Exception = ae;
                }
            else
                action(instance);
        }

        public TReturn Query<T, TReturn>(Func<T, TReturn> func) where T : class where TReturn : class
        {
            var instance = _container.GetInstance<T>();

            if (ScenarioContext.Current.ScenarioInfo.Tags.Contains("error"))
                try
                {
                    return func(instance);
                }
                catch (ApplicationException ae)
                {
                    Exception = ae;
                    return null;
                }

            return func(instance);
        }

        public void Dispose()
        {
            Transaction.Rollback();
            Db.Dispose();
            _scope.Dispose();
        }
    }
}