using System.Linq;
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
            var allActions = typeof (Api.Actions.Notes.Create)
                .Assembly
                .GetExportedTypes()
                .Where(t => t.Namespace != null 
                    && t.Namespace.StartsWith("FTF.Api.Actions"));

            foreach (var action in allActions)
                _container.RegisterInstanceAs(_context.Container.GetInstance(action));
        }
    }
}
