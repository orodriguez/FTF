using System;
using System.Linq;
using FTF.Api.Exceptions;
using Xunit;

namespace FTF.Tests.XUnit.Notes
{
    public class CreateTest : UserAuthenticatedTest
    {
        [Fact]
        public void SimpleNote()
        {
            CurrentTime = new DateTime(2016, 2, 20);

            var noteId = App.Notes.Create("I was born");
            var note = App.Notes.Retrieve(noteId);

            Assert.Equal("I was born", note.Text);
            Assert.Equal(new DateTime(2016, 2, 20), note.CreationDate);
            Assert.Equal("orodriguez", note.UserName);
        }

        [Fact]
        public void WithTagsWithinText()
        {
            var noteId = App.Notes.Create("#Buy cheese at #SuperMarket");
            var note = App.Notes.Retrieve(noteId);

            Assert.Equal(new[]
            {
                "Buy",
                "SuperMarket"
            }, note.Tags.Select(t => t.Name));
        }

        [Fact]
        public void WithRedundantTagInTextAndList()
        {
            var noteId = App.Notes.Create("Note with #Tag1");

            var note = App.Notes.Retrieve(noteId);

            Assert.Equal(new[] { "Tag1" }, note.Tags.Select(t => t.Name));
        }

        [Fact]
        public void EmptyNote()
        {
            var exception = Assert.Throws<ValidationException>(() => App.Notes.Create(""));

            Assert.Equal("Note can not be empty", exception.Message);
        }
    }
}
