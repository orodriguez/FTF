using System.Collections.Generic;
using FTF.Api.Actions.Tags;
using FTF.Api.Responses;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using static NUnit.Framework.Assert;

namespace FTF.Specs.Steps
{
    [Binding]
    public class ListTagsSteps
    {
        private IEnumerable<ITag> _response;

        private readonly Create _createTag;

        private readonly ListAll _listTags;

        private readonly ListJoint _listJointTags;

        public ListTagsSteps(Create createTag, ListAll listTags, ListJoint listJointTags)
        {
            _createTag = createTag;
            _listTags = listTags;
            _listJointTags = listJointTags;
        }

        [Given(@"I created a tag with name '(.*)'")]
        public void CreateTag(string name) => _createTag(name);

        [When(@"I list all tags")]
        public void ListTags() => _response = _listTags();

        [When(@"I list all tags that joint the tag '(.*)'")]
        public void ListJointTags(string tagName) => _response = _listJointTags(tagName);

        [Then(@"the tags list should match:")]
        public void TagsShouldMatch(Table table) => table.CompareToSet(_response);

        [Then(@"the tags list should be empty")]
        public void ListShouldBeEmpty() => IsEmpty(_response);
    }
}