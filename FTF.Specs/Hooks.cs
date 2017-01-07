using BoDi;
using FTF.Api.Actions.Notes;
using TechTalk.SpecFlow;

namespace FTF.Specs
{
    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer _container;

        private readonly Context _context;

        public Hooks(IObjectContainer container, Context context)
        {
            _container = container;
            _context = context;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _container.RegisterInstanceAs(_context.Container.GetInstance<Create>());
        }
    }
}
