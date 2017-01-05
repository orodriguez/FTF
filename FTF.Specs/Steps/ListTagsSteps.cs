using System.Collections.Generic;
using System.Data.Entity;
using FTF.Api.Actions.Tags;
using FTF.Api.Responses;
using FTF.Core;
using FTF.Core.QueriableFilters;
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

        [Given(@"I created a tag with name '(.*)'")]
        public void CreateTag(string name)
        {
            Create create = new CreateTagHandler(
                save: tag => _context.Db.Tags.Add(tag), 
                saveChanges: () => _context.Db.SaveChanges()
            ).Create;

            create(name);
        }

        [When(@"I list all tags")]
        public void ListTags()
        {
            ListAll listAll = new Queries(
                taggetNotes: new TagginsFilteredByUser(_context.Db.TaggedNotes, () => _context.CurrentUser.Id), 
                getCurrentUserId: () => _context.CurrentUser.Id
            ).ListAll;

            _response = listAll();
        }

        [When(@"I list all tags that joint the tag '(.*)'")]
        public void ListJointTags(string tagName)
        {
            ListJoint listJoint = new Queries(
                taggetNotes: _context.Db.TaggedNotes,
                getCurrentUserId: () => _context.CurrentUser.Id
            ).ListJoint;

            _response = listJoint(tagName);
        }

        [Then(@"the tags list should match:")]
        public void TagsShouldMatch(Table table) => table.CompareToSet(_response);

        [Then(@"the tags list should be empty")]
        public void ListShouldBeEmpty() => IsEmpty(_response);
    }
}