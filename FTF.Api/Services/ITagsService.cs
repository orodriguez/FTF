using System.Collections.Generic;
using FTF.Api.Responses;

namespace FTF.Api.Services
{
    public interface ITagsService
    {
        int Create(string tagName);

        IEnumerable<ITag> All();
    }
}