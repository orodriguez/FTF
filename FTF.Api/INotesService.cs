using FTF.Api.Responses;

namespace FTF.Api
{
    public interface INotesService
    {
        int Create(string text);
        INote Retrieve(int noteId);
        void Update(int id, string text);
        void Delete(int noteId);
    }
}