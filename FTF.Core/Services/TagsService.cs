using System.Collections.Generic;
using FTF.Api.Responses;
using FTF.Api.Services;
using FTF.Core.Attributes;

namespace FTF.Core.Services
{
    [Role(typeof(ITagsService))]
    public class TagsService : ITagsService
    {
        private readonly Tags.Queries _queries;

        public TagsService(Tags.Queries queries)
        {
            _queries = queries;
        }

        public IEnumerable<ITag> All() => _queries.ListAll();

        public IEnumerable<ITag> Joint(string tag) => _queries.ListJoint(tag);
    }
}