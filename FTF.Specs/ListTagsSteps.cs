using System.Collections.Generic;
using FTF.Core;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace FTF.Specs
{
    [Binding]
    public class ListTagsSteps
    {
        private IEnumerable<Tag> _tags;

        [When(@"I list all tags")]
        public void ListTags()
        {
            _tags = new TagQueries().All();
        }

        [Then(@"the tags list should match:")]
        public void TagsShouldMatch(Table table) => table.CompareToSet(_tags);
    }
}