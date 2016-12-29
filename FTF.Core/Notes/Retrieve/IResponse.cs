using System;
using System.Collections.Generic;

namespace FTF.Core.Notes.Retrieve
{
    public interface IResponse
    {
        int Id { get; }
        string Text { get; }
        DateTime CreationDate { get; }
        string UserName { get; }
        ICollection<Tag> Tags { get; }
    }
}