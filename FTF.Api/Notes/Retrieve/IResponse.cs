using System;
using System.Collections.Generic;

namespace FTF.Api.Notes.Retrieve
{
    public interface IResponse
    {
        int Id { get; }
        string Text { get; }
        DateTime CreationDate { get; }
        string UserName { get; }
        ICollection<ITag> Tags { get; }
    }
}