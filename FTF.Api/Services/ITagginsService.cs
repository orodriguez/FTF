using System.Collections.Generic;
using FTF.Api.Responses;

namespace FTF.Api.Services
{
    public interface ITagginsService
    {
        IEnumerable<ITag> All();

        IEnumerable<ITag> Joint(string tag);
    }
}