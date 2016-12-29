namespace FTF.Api.Responses
{
    public interface ITag
    {
        int Id { get; set; }
        string Name { get; set; }
        int NotesCount { get; }
    }
}