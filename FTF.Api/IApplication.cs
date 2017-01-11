using System;

namespace FTF.Api
{
    public interface IApplication : IDisposable
    {
        IAuthService Auth { get; set; }

        INotesService Notes { get; set; }

        ITagsService Tags { get; set; }
    }
}