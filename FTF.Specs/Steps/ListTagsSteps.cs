using System.Collections.Generic;
using FTF.Api.Actions.Tags;
using FTF.Api.Responses;
using FTF.Core.QueriableFilters;
using FTF.Core.Tags;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using static NUnit.Framework.Assert;

namespace FTF.Specs.Steps
{
    [Binding]
    public class ListTagsSteps : Steps
    {
        private IEnumerable<ITag> _response;

        public ListTagsSteps(Context context) : base(context)
        {
        }

        [Given(@"I created a tag with name '(.*)'")]
        public void CreateTag(string name) => Exec<Create>(f => f(name));

        [When(@"I list all tags")]
        public void ListTags() => _response = Query<ListAll, IEnumerable<ITag>>(f => f());

        [When(@"I list all tags that joint the tag '(.*)'")]
        public void ListJointTags(string tagName) => _response = Query<ListJoint, IEnumerable<ITag>>(f => f(tagName));

        [Then(@"the tags list should match:")]
        public void TagsShouldMatch(Table table) => table.CompareToSet(_response);

        [Then(@"the tags list should be empty")]
        public void ListShouldBeEmpty() => IsEmpty(_response);
    }
}