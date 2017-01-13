using System;
using System.Data.Entity;
using FTF.Api.Services;
using SimpleInjector;
using DbContext = FTF.Core.EntityFramework.DbContext;

namespace FTF.IoC.SimpleInjector
{
    public class Application : IApplication
    {
        private readonly IDisposable _scope;

        private readonly Container _container;

        private readonly DbContextTransaction _trans;

        public Application(Container container)
        {
            _container = container;

            _scope = container.BeginLifetimeScope();

            _trans = container.GetInstance<DbContext>()
                .Database
                .BeginTransaction();
        }

        public IAuthService Auth => _container.GetInstance<IAuthService>();

        public INotesService Notes => _container.GetInstance<INotesService>();

        public ITagsService Tags => _container.GetInstance<ITagsService>();

        public ITagginsService Taggins => _container.GetInstance<ITagginsService>();

        public void Dispose()
        {
            _trans.Rollback();
            _scope.Dispose();
        }
    }
}