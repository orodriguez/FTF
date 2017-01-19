using System;
using System.Linq;

namespace FTF.Core.Predicates
{
    internal static class GenericPredicates
    {
        public static Func<T, bool> NotIn<T>(IQueryable<T> @group) where T : class => 
            obj => @group.All(objInGroup => obj != objInGroup);
    }
}