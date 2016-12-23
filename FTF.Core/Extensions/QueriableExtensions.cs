using System.Linq;

namespace FTF.Core.Extensions
{
    public static class QueriableExtensions
    {
        public static int NextId(this IQueryable<Note> notes) => 
            notes.Any() ? notes.Max(_ => _.Id) : 1;
    }
}