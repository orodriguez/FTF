using System.Linq;
using Xunit;

namespace FTF.Tests.XUnit.Tags
{
    public class CreateTest : UserAuthenticatedTest
    {
        [Fact]
        public void Simple()
        {
            App.Tags.Create("SomeTag");

            var tags = App.Tags.All();

            Assert.Equal(new[] { "SomeTag" }, tags.Select(t => t.Name));
        }
    }
}
