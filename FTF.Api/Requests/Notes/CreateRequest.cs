namespace FTF.Api.Requests.Notes
{
    public class CreateRequest
    {
        public string Text { get; set; }
        public string[] Tags { get; set; }

        public CreateRequest()
        {
            Tags = new string[0];
        }
    }
}