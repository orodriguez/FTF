using System;

namespace FTF.Api.Services
{
    public interface IApplication : IDisposable
    {
        IAuthService Auth { get; }

        INotesService Notes { get; }

        ITagsService Tags { get; }
    }
}