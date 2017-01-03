using System.Collections.Generic;
using FTF.Api.Actions.Tags;
using FTF.Api.Responses;
using FTF.Core.Tags;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using static NUnit.Framework.Assert;

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
            ListAll listAll = new Queries(
                tags: _context.Db.Tags, 
                getCurrentUserId: () => _context.CurrentUser.Id
            ).ListAll;

            _response = listAll();
        }

        [Then(@"the tags list should match:")]
        public void TagsShouldMatch(Table table) => table.CompareToSet(_response);

        [Then(@"the tags list should be empty")]
        public void ListShouldBeEmpty() => IsEmpty(_response);
    }
}