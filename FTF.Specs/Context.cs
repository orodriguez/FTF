using System;
using System.Data.Entity;
using System.Linq;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using SimpleInjector;
using DbContext = FTF.Storage.EntityFramework.DbContext;
using FTF.Core.Extensions.Queriable;
using FTF.IoC.SimpleInjector;
using TechTalk.SpecFlow;

namespace FTF.Specs
{
    public class Context : IDisposable
    {
        private User _currentUser;

        public DbContext Db => Container.GetInstance<DbContext>();

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

        public Container Container { get; }

        private readonly Scope _scope;

        public GenerateNoteId NextId { get; set; }

        public Context()
        {
            GetCurrentDate = () => DateTime.Now;
            NextId = () => Container.GetInstance<IQueryable<Note>>().NextId();

            Container = new Container();
            Container.RegisterTypes(generateNoteId: () => NextId(),
                getCurrentDate: () => GetCurrentDate(),
                getCurrentUser: () => CurrentUser,
                getCurrentUserId: () => CurrentUser.Id,
                setCurrentUser: user => CurrentUser = user);

            _scope = Container.BeginLifetimeScope();

            Transaction = Container.GetInstance<DbContext>().Database.BeginTransaction();
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
            var instance = Container.GetInstance<T>();

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

        public TReturn Catch<TReturn>(Func<TReturn> func) 
            where TReturn : class
        {
            if (ScenarioContext.Current.ScenarioInfo.Tags.Contains("error"))
                try
                {
                    return func();
                }
                catch (ApplicationException ae)
                {
                    Exception = ae;
                    return null;
                }

            return func();
        }

        public void Catch(Action action)
        {
            if (ScenarioContext.Current.ScenarioInfo.Tags.Contains("error"))
                try { action(); }
                catch (ApplicationException ae) { Exception = ae; }
            else
                action();
        }

        public void Dispose()
        {
            Transaction.Rollback();
            Db.Dispose();
            _scope.Dispose();
        }
    }
}