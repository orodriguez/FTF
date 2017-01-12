using System;
using FTF.Api.Services;

namespace FTF.Api
{
    public interface IApplication : IDisposable
    {
        IAuthService Auth { get; }

        INotesService Notes { get; }

        ITagsService Tags { get; }
    }
}