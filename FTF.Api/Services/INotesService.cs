using System.Collections.Generic;
using FTF.Api.Requests.Notes;
using FTF.Api.Responses;

namespace FTF.Api.Services
{
    public interface INotesService
    {
        int Create(string text);
        int Create(CreateRequest request);
        INote Retrieve(int noteId);
        void Update(int id, string text);
        void Delete(int noteId);
        IEnumerable<INote> All();
        IEnumerable<INote> ByTag(string tagName);
    }
}