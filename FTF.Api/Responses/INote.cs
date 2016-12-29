using System;
using System.Collections.Generic;

namespace FTF.Api.Responses
{
    public interface INote
    {
        int Id { get; }
        string Text { get; }
        DateTime CreationDate { get; }
        string UserName { get; }
        IEnumerable<ITag> Tags { get; }
    }
}