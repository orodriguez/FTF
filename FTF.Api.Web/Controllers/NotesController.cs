using System.Collections.Generic;
using System.Web.Http;
using FTF.Api.Requests.Notes;
using FTF.Api.Responses;
using FTF.Api.Services;

namespace FTF.Api.Web.Controllers
{
    public class NotesController : ApiController
    {
        private readonly INotesService _notes;

        public NotesController(INotesService notes)
        {
            _notes = notes;
        }

        public int Post(CreateRequest request) => _notes.Create(request);

        public INote Get(int noteId) => _notes.Retrieve(noteId);

        public void Update(int id, UpdateRequest request) => _notes.Update(id, request);

        public void Delete(int noteId) => _notes.Delete(noteId);

        public IEnumerable<INote> Get() => _notes.All();

        public IEnumerable<INote> Get(string tagName) => _notes.ByTag(tagName);
    }
}
