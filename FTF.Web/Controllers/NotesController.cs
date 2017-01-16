using System.Collections.Generic;
using System.Web.Http;
using FTF.Api.Requests.Notes;
using FTF.Api.Responses;
using FTF.Api.Services;

namespace FTF.Web.Controllers
{
    public class NotesController : ApiController
    {
        private readonly INotesService _notes;

        public NotesController(INotesService notes)
        {
            _notes = notes;
        }

        public int Post(CreateRequest request) => _notes.Create(request);

        public INote Get(int id) => _notes.Retrieve(id);

        public void Update(int id, UpdateRequest request) => _notes.Update(id, request);

        public void Delete(int id) => _notes.Delete(id);

        public IEnumerable<INote> Get() => _notes.All();

        public IEnumerable<INote> Get(string tagName) => _notes.ByTag(tagName);
    }
}
