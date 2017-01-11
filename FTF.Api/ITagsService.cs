using System.Collections.Generic;
using FTF.Api.Responses;

namespace FTF.Api
{
    public interface ITagsService
    {
        IEnumerable<ITag> All();
    }
}