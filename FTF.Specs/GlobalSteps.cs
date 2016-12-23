using System;
using TechTalk.SpecFlow;

namespace FTF.Specs
{
    [Binding]
    public class GlobalSteps
    {
        private readonly Context _context;

        public GlobalSteps(Context context)
        {
            _context = context;
        }

        [Given(@"today is '(.*)'")]
        public void TodayIs(string date) => _context.GetCurrentDate = () => DateTime.Parse(date);
    }
}