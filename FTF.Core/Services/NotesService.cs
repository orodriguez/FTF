using FTF.Api;
using FTF.Api.Actions.Notes;
using FTF.Api.Responses;

namespace FTF.Core.Services
{
    public class NotesService : INotesService
    {
        private readonly Create _create;

        private readonly Retrieve _retrieve;

        private readonly Update _update;

        private readonly Delete _delete;

        public NotesService(Create create, Retrieve retrieve, Update update, Delete delete)
        {
            _create = create;
            _retrieve = retrieve;
            _delete = delete;
            _update = update;
        }

        public int Create(string text) => _create(text);

        public INote Retrieve(int noteId) => _retrieve(noteId);
        public void Update(int id, string text) => _update(id, text);

        public void Delete(int noteId) => _delete(noteId);
    }
}