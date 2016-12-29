using System;
using System.Collections.Generic;

namespace FTF.Core.Notes.Retrieve
{
    public interface IResponse
    {
        int Id { get; set; }
        string Text { get; set; }
        DateTime CreationDate { get; set; }
        ICollection<Tag> Tags { get; set; }
    }
}