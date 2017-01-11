using System.Collections.Generic;
using System.Linq;
using FTF.Api.Responses;
using Xunit;

namespace FTF.Tests.XUnit.Tags
{
    public class ListJoint : UserAuthenticatedTest
    {
        [Fact]
        public void _1Note_3Tags_Read1_Piano1_Empty0()
        {
            App.Notes.Create("Empty");
            App.Notes.Create("#Read a book about #Piano performance");

            var tags = App.Tags.Joint("Read").ToArray();

            Assert.Equal(new[] { "Read", "Piano" }, tags.Select(t => t.Name));
            Assert.Equal(new[] { 1, 1 }, tags.Select(t => t.NotesCount));
        }

        [Fact]
        public void _3Notes_3Tags_Read1_3_Programming1_2_FTF2_3()
        {
            App.Notes.Create("#Read article about #Programming");
            App.Notes.Create("Write sample application #Programming #FTF");
            App.Notes.Create("#FTF #Read about design principles");

            var tags = App.Tags.Joint("Read").ToArray();

            Assert.Equal(new[] { "Read", "Programming", "FTF" }, tags.Select(t => t.Name));
            Assert.Equal(new[] { 2, 1, 1 }, tags.Select(t => t.NotesCount));
        }

        [Fact]
        public void _2Notes_3Tags_Read1_Programming1_Mary2()
        {
            App.Notes.Create("#Buy tire #Car");
            App.Notes.Create("#Buy gift for #Mary");

            var tags = App.Tags.Joint("Mary").ToArray();

            Assert.Equal(new[] { "Buy", "Mary" }, tags.Select(t => t.Name));
            Assert.Equal(new[] { 1, 1 }, tags.Select(t => t.NotesCount));
        }
    }
}
