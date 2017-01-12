using System.Collections.Generic;
using System.Linq;
using FTF.Api.Responses;
using FTF.Api.Services;
using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.EntityFramework;

namespace FTF.Core.Services
{
    [Role(typeof(ITagsService))]
    public class TagsService : ITagsService
    {
        private readonly DbContext _db;

        private readonly GetCurrentUser _getCurrentUser;

        public TagsService(DbContext db, GetCurrentUser getCurrentUser)
        {
            _db = db;
            _getCurrentUser = getCurrentUser;
        }

        public int Create(string name)
        {
            var tag = _db.Tags.Add(new Tag
            {
                Name = name,
                User = _getCurrentUser()
            });

            _db.SaveChanges();

            return tag.Id;
        }

        public IEnumerable<ITag> All() => _db.Tags;
    }
}
