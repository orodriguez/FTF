using FTF.Api.Exceptions;
using Xunit;

namespace FTF.Tests.XUnit.Notes
{
    public class RetrieveTest : UserAuthenticatedTest
    {
        [Fact]
        public void NoteFromAnotherUser()
        {
            var noteId = App.Notes.Create("A Note");

            App.Auth.SignUp("anotheruser");
            App.Auth.SignIn("anotheruser");

            var exception = Assert.Throws<RecordNotFoundException>(() => App.Notes.Retrieve(noteId));
            Assert.Equal($"Note with id #{noteId} does not exist", exception.Message);
        }

        [Fact]
        public void NoteNotFound()
        {
            var error = Assert.Throws<RecordNotFoundException>(() => App.Notes.Retrieve(101));
            Assert.Equal("Note with id #101 does not exist", error.Message);
        }
    }
}
