using System;
using System.Runtime.Serialization;

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

        protected TReturn Query<T, TReturn>(Func<T, TReturn> func) 
            where T : class
            where TReturn : class => Context.Query(func);

        protected Exception Exception => Context.Exception;
    }
}