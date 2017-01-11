using FTF.Api.Responses;
using FTF.Api.Services;
using FTF.Core.Notes;

namespace FTF.Core.Services
{
    public class NotesService : INotesService
    {
        private readonly CreateHandler _create;

        private readonly Notes.Queries _queries;

        private readonly UpdateHandler _update;

        private readonly DeleteHandler _delete;

        public NotesService(CreateHandler create, Notes.Queries queries, UpdateHandler update, DeleteHandler delete)
        {
            _create = create;
            _queries = queries;
            _delete = delete;
            _update = update;
        }

        public int Create(string text) => _create.Create(text);

        public INote Retrieve(int noteId) => _queries.Retrieve(noteId);

        public void Update(int id, string text) => _update.Update(id, text);

        public void Delete(int noteId) => _delete.Delete(noteId);
    }
}