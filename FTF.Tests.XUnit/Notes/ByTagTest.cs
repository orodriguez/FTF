using System;
using System.Data.Entity.Core.Objects;
using FTF.Api.Requests;
using FTF.Api.Requests.Notes;
using Xunit;

namespace FTF.Tests.XUnit.Notes
{
    public class ByTagTest : UserAuthenticatedTest
    {
        [Fact(Skip = "Will implement update tags first")]
        public void OrderByTaggingDateDesc()
        {
            CurrentTime = new DateTime(2016, 2, 14);
            var noteId = App.Notes.Create("Go to supermarket");

            CurrentTime = new DateTime(2016, 2, 15);
            App.Notes.Create("#Buy book");

            CurrentTime = new DateTime(2016, 2, 16);
            App.Notes.Update(noteId, new UpdateRequest
            {
                Tags = new[] { "Buy" }
            });

            App.Notes.ByTag("Buy");
        }
    }
}
