namespace FTF.Api.Requests.Notes
{
    public class UpdateRequest
    {
        public string[] Tags { get; set; }
        public string Text { get; set; }

        public UpdateRequest()
        {
            Tags = new string[0];
        }
    }
}