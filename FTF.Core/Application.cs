using FTF.Api;

namespace FTF.Core
{
    public class Application : IApplication
    {
        public IAuthService AuthService { get; set; }

        public INotesService Notes { get; set; }

        public ITagsService Tags { get; set; }

        private readonly IStoragePort _storage;

        public Application(
            IAuthService authService, 
            INotesService notes, 
            ITagsService tags, 
            IStoragePort storage)
        {
            AuthService = authService;
            Notes = notes;
            Tags = tags;
            _storage = storage;
        }

        public void Dispose() => _storage.Dispose();
    }
}