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

        protected void Exec<T>(Action<T> action) where T : class => Context.Exec(action);

        protected TReturn Query<T, TReturn>(Func<T, TReturn> func) 
            where T : class
            where TReturn : class => Context.Query(func);

        protected ApplicationException Exception => Context.Exception;
    }
}