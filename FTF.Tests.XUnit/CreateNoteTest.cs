using System;
using FTF.Api;
using FTF.IoC.SimpleInjector;
using Xunit;

namespace FTF.Tests.XUnit
{
    public class CreateNoteTest : IDisposable
    {
        private readonly IApplication _app;

        public CreateNoteTest()
        {
            _app = new ApplicationFactory()
                .Make(getCurrentDate: () => new DateTime(2016, 2, 20));

            _app.AuthService.SignUp("orodriguez");
            _app.AuthService.SignIn("orodriguez");
        }

        public void Dispose() => _app.Dispose();

        [Fact]
        public void SimpleNote()
        {
            var noteId = _app.Notes.Create("I was born");
            var note = _app.Notes.Retrieve(noteId);

            Assert.Equal("I was born", note.Text);
            Assert.Equal(new DateTime(2016, 2, 20), note.CreationDate);
            Assert.Equal("orodriguez", note.UserName);
        }
    }
}
