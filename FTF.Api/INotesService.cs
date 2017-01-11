using FTF.Api.Responses;

namespace FTF.Api
{
    public interface INotesService
    {
        int Create(string text);
        INote Retrieve(int noteId);
        void Delete(int noteId);
    }
}