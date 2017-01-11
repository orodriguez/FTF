using FTF.Api;

namespace FTF.Core
{
    public class Application : IApplication
    {
        public IAuthService AuthService { get; set; }

        public INotesService Notes { get; set; }

        private readonly IStoragePort _storage;

        public Application(
            IAuthService authService, 
            INotesService notes, 
            IStoragePort storage)
        {
            AuthService = authService;
            Notes = notes;
            _storage = storage;
        }

        public void Dispose() => _storage.Dispose();
    }
}