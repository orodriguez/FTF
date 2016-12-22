using TechTalk.SpecFlow;

namespace FTF.Specs
{
    [Binding]
    public class Steps
    {
        [Given(@"today is '(.*)'")]
        public void TodayIs(string date)
        {
        }

        [Given(@"I created a note \#(.*) with text '(.*)'")]
        public void CreateNote(int id, string text)
        {
        }

        [When(@"I retrieve the note \#(.*)")]
        public void RetrieveNote(int id)
        {
        }

        [Then(@"the note should match:")]
        public void NoteShouldMatch(Table table)
        {
        }
    }
}
