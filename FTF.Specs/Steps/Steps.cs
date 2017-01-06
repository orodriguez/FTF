using System;
using System.Linq;
using FTF.Api.Actions.Notes;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.Extensions.Queriable;
using FTF.Core.Notes;
using FTF.Storage.EntityFramework;
using SimpleInjector;

namespace FTF.Specs.Steps
{
    public class Steps
    {
        protected Context Context;

        private readonly Container _container = new Container();

        public Steps(Context context)
        {
            Context = context;

            _container.Register<Create>(() => _container.GetInstance<CreateHandler>().Create);
            _container.Register<CreateHandler>();
            _container.Register<GenerateNoteId>(() => _container.GetInstance<IQueryable<Note>>().NextId);
            _container.Register(() => Context.GetCurrentDate);
            _container.Register<Save<Note>>(() => e => _container.GetInstance<DbContext>().Notes.Add(e));
            _container.Register<SaveChanges>(() => _container.GetInstance<DbContext>().SaveChanges);
            _container.Register<IQueryable<Tag>>(() => _container.GetInstance<DbContext>().Tags);
            _container.Register<GetCurrentUser>(() => () => Context.CurrentUser);
            _container.Register<ValidateNote>(() => NoteValidator.Validate);
            _container.Register<IQueryable<Note>>(() => _container.GetInstance<DbContext>().Notes);
            _container.Register(() => new DbContext("name=FTF.Tests", new System.Data.Entity.DropCreateDatabaseAlways<DbContext>()));
        }

        protected void Exec<T>(Action<T> action) where T : class =>
            Context.StoreException(() => action(_container.GetInstance<T>()));
    }
}