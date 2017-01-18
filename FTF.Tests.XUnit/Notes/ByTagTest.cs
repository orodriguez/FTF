using System;
using System.Linq;
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
            App.Notes.Update(noteId, "#Buy Go to supermarkert");

            var notes = App.Notes.ByTag("Buy");

            Assert.Equal(new[]
            {
                "#Buy Go to supermarkert",
                "#Buy book"
            }, notes.Select(n => n.Text));
        }
    }
}
