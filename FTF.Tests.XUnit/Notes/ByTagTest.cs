using System;
using System.Linq;
using FTF.Api.Requests.Notes;
using Xunit;

namespace FTF.Tests.XUnit.Notes
{
    public class ByTagTest : UserAuthenticatedTest
    {
        [Fact]
        public void OrderByTaggingDateDesc()
        {
            CurrentTime = new DateTime(2016, 2, 14);
            var noteId = App.Notes.Create("Go to supermarket");

            CurrentTime = new DateTime(2016, 2, 15);
            App.Notes.Create("#Buy book");

            CurrentTime = new DateTime(2016, 2, 16);
            App.Notes.Update(noteId, new UpdateRequest
            {
                Tags = new[] { "Buy" }
            });

            var notes = App.Notes.ByTag("Buy");

            Assert.Equal(new[]
            {
                "Go to supermarket",
                "#Buy book"
            }, notes.Select(n => n.Text));
        }
    }
}
