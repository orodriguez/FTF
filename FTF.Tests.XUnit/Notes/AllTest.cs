using System;
using System.Linq;
using Xunit;

namespace FTF.Tests.XUnit.Notes
{
    public class AllTest : UserAuthenticatedTest
    {
        [Fact]
        public void ShouldSortInCreationOrderDesc()
        {
            CurrentTime = new DateTime(2016, 2, 14);
            App.Notes.Create("Note 1");

            CurrentTime = new DateTime(2016, 2, 15);
            App.Notes.Create("Note 2");

            CurrentTime = new DateTime(2016, 2, 20);
            App.Notes.Create("Note 3");

            var notes = App.Notes.All();

            Assert.Equal(new [] { "Note 3", "Note 2", "Note 1" }, notes.Select(n => n.Text));
        }
    }
}
