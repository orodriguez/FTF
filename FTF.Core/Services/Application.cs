using FTF.Api;
using FTF.Api.Services;
using FTF.Core.Attributes;
using FTF.Core.Ports;

namespace FTF.Core.Services
{
    [Role(typeof(IApplication))]
    public class Application : IApplication
    {
        public IAuthService Auth { get; set; }

        public INotesService Notes { get; set; }

        public ITagsService Tags { get; set; }

        public ITagginsService Taggins { get; set; }

        private readonly IStorage _storage;

        public Application(
            IAuthService authService, 
            INotesService notes, 
            ITagsService tags, 
            ITagginsService taggins, 
            IStorage storage)
        {
            Auth = authService;
            Notes = notes;
            Tags = tags;
            Taggins = taggins;
            _storage = storage;
        }

        public void Dispose() => _storage.Dispose();
    }
}