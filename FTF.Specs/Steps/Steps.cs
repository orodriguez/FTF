using System;
using FTF.Api.Actions.Notes;
using FTF.Core.Notes;
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
        }

        protected void Exec<T>(Action<T> action) where T : class => 
            action(_container.GetInstance<T>());
    }
}