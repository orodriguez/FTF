using System.Collections.Generic;
using System.Linq;

namespace FTF.Core.Extensions.Queriable
{
    internal static class TagQueries
    {
        public static IQueryable<Tag> ByName(this IQueryable<Tag> source, IEnumerable<string> names) =>
            source.Where(t => names.Contains(t.Name));
    }
}