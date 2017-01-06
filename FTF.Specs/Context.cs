using System;
using System.Data.Entity;
using System.Linq;
using FTF.Api.Actions.Notes;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.Notes;
using SimpleInjector;
using DbContext = FTF.Storage.EntityFramework.DbContext;
using FTF.Core.Extensions.Queriable;
using SimpleInjector.Extensions.LifetimeScoping;

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
            _container.Register<Save<Note>>(() => e => _container.GetInstance<DbContext>().Notes.Add(e));
            _container.Register<SaveChanges>(() => _container.GetInstance<DbContext>().SaveChanges);
            _container.Register<IQueryable<Tag>>(() => _container.GetInstance<DbContext>().Tags);
            _container.Register<GetCurrentUser>(() => () => CurrentUser);
            _container.Register<ValidateNote>(() => NoteValidator.Validate);
            _container.Register<IQueryable<Note>>(() => _container.GetInstance<DbContext>().Notes);
            _container.Register(() => new DbContext("name=FTF.Tests", new DropCreateDatabaseAlways<DbContext>()), Lifestyle.Scoped);

            _scope = _container.BeginLifetimeScope();
            Transaction = _container.GetInstance<DbContext>().Database.BeginTransaction();
        }

        public void StoreException(Action action)
        {
            try
            {
                action();
            }
            catch (ApplicationException e)
            {
                Exception = e;
            }
        }

        public void Exec<T>(Action<T> action) where T : class => 
            StoreException(() => action(_container.GetInstance<T>()));

        public void Dispose()
        {
            Transaction.Rollback();
            Db.Dispose();
            _scope.Dispose();
        }
    }
}