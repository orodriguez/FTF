using System.Collections.Generic;
using FTF.Api.Actions.Tags;
using FTF.Api.Responses;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace FTF.Specs.Steps
{
    [Binding]
    public class ListTagsSteps
    {
        private readonly Context _context;

        private IEnumerable<ITag> _response;

        public ListTagsSteps(Context context)
        {
            _context = context;
        }

        [When(@"I list all tags")]
        public void ListTags()
        {
            List list = () => _context.Db.Tags;

            _response = list();
        }

        [Then(@"the tags list should match:")]
        public void TagsShouldMatch(Table table) => table.CompareToSet(_response);
    }
}