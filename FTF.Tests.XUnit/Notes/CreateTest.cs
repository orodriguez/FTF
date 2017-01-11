using System;
using System.Linq;
using FTF.Api;
using FTF.Api.Exceptions;
using FTF.IoC.SimpleInjector;
using Xunit;

namespace FTF.Tests.XUnit.Notes
{
    public class CreateTest : IDisposable
    {
        private readonly IApplication _app;

        public CreateTest()
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

        [Fact]
        public void WithTags()
        {
            var noteId = _app.Notes.Create("#Buy cheese at #SuperMarket");
            var note = _app.Notes.Retrieve(noteId);

            Assert.Equal(new[]
            {
                "Buy",
                "SuperMarket"
            }, note.Tags.Select(t => t.Name));
        }

        [Fact]
        public void EmptyNote()
        {
            var exception = Assert.Throws<ValidationException>(() => _app.Notes.Create(""));

            Assert.Equal("Note can not be empty", exception.Message);
        }
    }
}
