using System;
using Xunit;

namespace FTF.Tests.XUnit
{
    public class CreateNoteTest : ApiTest
    {
        public CreateNoteTest()
        {
            TodayIs(1984, 2, 20);
            SignUp("orodriguez");
            SignIn("orodriguez");
        }

        [Fact]
        public void SimpleNote()
        {
            var noteId = CreateNotes("I was born");
            var note = RetrieveNote(noteId);

            Assert.Equal("I was born", note.Text);
            Assert.Equal(new DateTime(1984, 2, 20), note.CreationDate);
            Assert.Equal("orodriguez", note.UserName);
        }
    }
}
