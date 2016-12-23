using System.Collections.Generic;
using FTF.Core;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace FTF.Specs.Steps
{
    [Binding]
    public class ListTagsSteps
    {
        private readonly Context _context;

        private IEnumerable<Tag> _tags;

        public ListTagsSteps(Context context)
        {
            _context = context;
        }

        [When(@"I list all tags")]
        public void ListTags()
        {
            _tags = _context.Db.Tags;
        }

        [Then(@"the tags list should match:")]
        public void TagsShouldMatch(Table table) => table.CompareToSet(_tags);
    }
}