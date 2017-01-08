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

        protected TReturn Catch<TReturn>(Func<TReturn> func)
            where TReturn : class 
            => Context.Catch(func);

        protected void Catch(Action action)
            => Context.Catch(action);

        protected ApplicationException Exception => Context.Exception;
    }
}