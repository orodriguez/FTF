using BoDi;
using TechTalk.SpecFlow;

namespace FTF.Specs
{
    [Binding]
    public class Hooks
    {
        private IObjectContainer _container;

        public Hooks(IObjectContainer container, Context context)
        {
            _container = container;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
        }
    }
}
