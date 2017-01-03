namespace FTF.Api.Responses
{
    public interface ITag
    {
        int Id { get; }
        string Name { get; }
        int NotesCount { get; }
    }
}