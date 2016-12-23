using System.Linq;

namespace FTF.Core.Extensions.Queriable
{
    public static class NoteQueries
    {
        public static int NextId(this IQueryable<Note> notes) => 
            notes.Any() ? notes.Max(_ => _.Id) + 1 : 1;
    }
}