using System.Collections.Generic;
using System.Linq;
using FTF.Core.Entities;

namespace FTF.Core.Extensions.Queriable
{
    internal static class TagQueries
    {
        public static IQueryable<Tag> WhereNameContains(this IQueryable<Tag> source, IEnumerable<string> names) =>
            source.Where(t => names.Contains(t.Name));
    }
}