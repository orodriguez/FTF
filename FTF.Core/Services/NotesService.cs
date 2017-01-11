using FTF.Api;
using FTF.Api.Actions.Notes;
using FTF.Api.Responses;

namespace FTF.Core.Services
{
    public class NotesService : INotesService
    {
        private readonly Create _create;

        private readonly Retrieve _retrieve;

        public NotesService(Create create, Retrieve retrieve)
        {
            _create = create;
            _retrieve = retrieve;
        }

        public int Create(string text) => _create(text);

        public INote Retrieve(int noteId) => _retrieve(noteId);
    }
}