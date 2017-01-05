using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FTF.Core.Entities;

namespace FTF.Core.QueriableFilters
{
    public class TagginsFilteredByUser : IQueryable<Tagging>
    {
        private readonly IQueryable<Tagging> _queriable;

        public TagginsFilteredByUser(IQueryable<Tagging> taggins, Func<int> getCurrentUserId)
        {
            var userId = getCurrentUserId();

            _queriable = taggins
                .Where(t => t.Note.User.Id == userId);
        }

        public IEnumerator<Tagging> GetEnumerator() => _queriable.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Expression Expression => _queriable.Expression;
        public Type ElementType => _queriable.ElementType;
        public IQueryProvider Provider => _queriable.Provider;
    }
}