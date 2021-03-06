using System.Collections.Generic;
using System.Linq;
using FTF.Core.Entities;

namespace FTF.Core.Queries
{
    internal static class NameableQueries
    {
        public static IEnumerable<string> Names(this IEnumerable<INameable> source) => source.Select(o => o.Name);
    }
}