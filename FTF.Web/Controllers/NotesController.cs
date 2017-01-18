using System.Collections.Generic;
using System.Web.Http;
using FTF.Api.Responses;
using FTF.Api.Services;
using FTF.Web.Filters;

namespace FTF.Web.Controllers
{
    public class NotesController : ApiController
    {
        private readonly INotesService _notes;

        public NotesController(INotesService notes)
        {
            _notes = notes;
        }

        [ValidationExceptionFilter]
        public int Post(string text) => _notes.Create(text);

        public INote Get(int id) => _notes.Retrieve(id);

        public void Update(int id, string text) => _notes.Update(id, text);

        public void Delete(int id) => _notes.Delete(id);

        public IEnumerable<INote> Get() => _notes.All();

        public IEnumerable<INote> Get(string tagName) => _notes.ByTag(tagName);
    }
}
