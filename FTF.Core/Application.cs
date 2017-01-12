using FTF.Api;
using FTF.Api.Services;
using FTF.Core.Attributes;

namespace FTF.Core
{
    [Role(typeof(IApplication))]
    public class Application : IApplication
    {
        public IAuthService Auth { get; set; }

        public INotesService Notes { get; set; }

        public ITagsService Tags { get; set; }

        private readonly IStoragePort _storage;

        public Application(
            IAuthService authService, 
            INotesService notes, 
            ITagsService tags, 
            IStoragePort storage)
        {
            Auth = authService;
            Notes = notes;
            Tags = tags;
            _storage = storage;
        }

        public void Dispose() => _storage.Dispose();
    }
}