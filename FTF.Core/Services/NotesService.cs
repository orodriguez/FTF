using FTF.Api;
using FTF.Api.Actions.Notes;
using FTF.Api.Responses;

namespace FTF.Core.Services
{
    public class NotesService : INotesService
    {
        private readonly Create _create;

        private readonly Retrieve _retrieve;

        private readonly Delete _delete;

        public NotesService(Create create, Retrieve retrieve, Delete delete)
        {
            _create = create;
            _retrieve = retrieve;
            _delete = delete;
        }

        public int Create(string text) => _create(text);

        public INote Retrieve(int noteId) => _retrieve(noteId);

        public void Delete(int noteId) => _delete(noteId);
    }
}