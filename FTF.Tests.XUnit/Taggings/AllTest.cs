using System.Linq;
using Xunit;

namespace FTF.Tests.XUnit.Taggings
{
    public class AllTest : UserAuthenticatedTest
    {
        [Fact]
        public void _1NoteWithNewTag()
        {
            App.Notes.Create("#Read a book");

            var tags = App.Taggins.All();

            var firstTag = tags.First();
            Assert.Equal("Read", firstTag.Name);
            Assert.Equal(1, firstTag.NotesCount);
        }

        [Fact]
        public void _2NotesWithSameTag()
        {
            App.Notes.Create("#Buy cheese");
            App.Notes.Create("#Buy nachos");

            var tags = App.Taggins.All();

            var firstTag = tags.First();
            Assert.Equal("Buy", firstTag.Name);
            Assert.Equal(2, firstTag.NotesCount);
        }

        [Fact]
        public void _1TagFromDifferentUser()
        {
            App.Notes.Create("#Buy cheese");
            App.Auth.SignUp("anotheruser");
            App.Auth.SignIn("anotheruser");

            var tags = App.Taggins.All();

            Assert.Empty(tags);
        }
    }
}
