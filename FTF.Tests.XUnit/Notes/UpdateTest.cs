using System;
using FTF.Api.Exceptions;
using Xunit;

namespace FTF.Tests.XUnit.Notes
{
    public class UpdateTest : ApplicationTest
    {
        [Fact]
        public void SimpleTextUpdate()
        {
            var noteId = App.Notes.Create("Buy cheese");
            App.Notes.Update(noteId, "Buy american cheese");

            var note = App.Notes.Retrieve(noteId);

            Assert.Equal("Buy american cheese", note.Text);
            Assert.Equal(new DateTime(2016,2,20), note.CreationDate);
            Assert.Equal("orodriguez", note.UserName);
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
