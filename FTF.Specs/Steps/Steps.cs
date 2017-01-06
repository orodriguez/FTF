using System;

namespace FTF.Specs.Steps
{
    public class Steps
    {
        protected Context Context;

        public Steps(Context context)
        {
            Context = context;
        }

        protected void Exec<T>(Action<T> action) where T : class => Context.Exec<T>(action);
    }
}