using FTF.Core.Extensions;
using Xunit;

namespace FTF.Tests.Unit
{
    public class TagNameParserTest
    {
        [Theory]
        [InlineData("", new string[0])]
        [InlineData("#Learn kung fu", new[] { "Learn" })]
        [InlineData("#Buy cheese in the #SuperMarket", new[] { "Buy", "SuperMarket" })]
        [InlineData("#Read a book #about how to #Read", new[] { "Read", "about" })]
        public void GivenAText_TagNamesShouldEqual(string text, string[] expectedTagNames)
            => Assert.Equal(expectedTagNames, text.ParseTagNames());
    }
}
