using System.Linq;
using FTF.Api.Exceptions;
using Xunit;

namespace FTF.Tests.XUnit.Notes
{
    public class DeleteTest : UserAuthenticatedTest
    {
        [Fact]
        public void Simple()
        {
            var noteId = App.Notes.Create("I was born");
            App.Notes.Delete(noteId);

            var exception = Assert.Throws<RecordNotFoundException>(() => App.Notes.Retrieve(noteId));
            Assert.Equal($"Note with id #{noteId} does not exist", exception.Message);
        }

        [Fact]
        public void WithTag()
        {
            var noteId = App.Notes.Create("#Buy cheese");
            App.Notes.Delete(noteId);

            var tags = App.Taggins.All();

            var firstTag = tags.First();

            Assert.Equal("Buy", firstTag.Name);
            Assert.Equal(0, firstTag.NotesCount);
        }
        
    }
}
