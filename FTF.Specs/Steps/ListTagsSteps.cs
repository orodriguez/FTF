using System.Collections.Generic;
using FTF.Api.Responses;
using FTF.Core.Tags;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using static NUnit.Framework.Assert;
using List = FTF.Api.Actions.Tags.List;

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
            List list = new Queries(
                tags: _context.Db.Tags, 
                getCurrentUserId: () => _context.CurrentUser.Id
            ).List;

            _response = list();
        }

        [Then(@"the tags list should match:")]
        public void TagsShouldMatch(Table table) => table.CompareToSet(_response);

        [Then(@"the tags list should be empty")]
        public void ListShouldBeEmpty() => IsEmpty(_response);
    }
}