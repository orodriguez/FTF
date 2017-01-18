using System;
using System.Linq;
using FTF.Api.Exceptions;
using Xunit;

namespace FTF.Tests.XUnit.Notes
{
    public class UpdateTest : UserAuthenticatedTest
    {
        [Fact]
        public void SimpleTextUpdate()
        {
            CurrentTime = new DateTime(2016, 2, 20);

            var noteId = App.Notes.Create("Buy cheese");
            App.Notes.Update(noteId, "Buy american cheese");

            var note = App.Notes.Retrieve(noteId);

            Assert.Equal("Buy american cheese", note.Text);
            Assert.Equal(new DateTime(2016,2,20), note.CreationDate);
            Assert.Equal("orodriguez", note.UserName);
        }

        [Fact]
        public void WithTags()
        {
            var noteId = App.Notes.Create("Note without tag");
            App.Notes.Update(noteId, "A Note with #some #tags");

            var note = App.Notes.Retrieve(noteId);

            Assert.Equal(new[] { "some", "tags" }, note.Tags.Select(n => n.Name));
        }

        [Fact]
        public void WithExistingTags()
        {
            App.Notes.Create("Note with #Tag");
            var noteId = App.Notes.Create("Note without tag");
            App.Notes.Update(noteId, "Note update with #Tag");

            var tags = App.Taggins.All().ToList();

            Assert.Equal(new[] { "Tag" }, tags.Select(t => t.Name));
            Assert.Equal(new[] { 2 }, tags.Select(t => t.NotesCount));
        }

        [Fact]
        public void EmptyText()
        {
            var noteId = App.Notes.Create("Buy cheese");

            var error = Assert.Throws<ValidationException>(() => App.Notes.Update(noteId, ""));

            Assert.Equal("Note can not be empty", error.Message);
        }
    }
}
