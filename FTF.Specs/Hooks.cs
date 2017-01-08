using System.Linq;
using BoDi;
using TechTalk.SpecFlow;

namespace FTF.Specs
{
    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer _specFlowContainer;

        private readonly Context _context;

        public Hooks(IObjectContainer specFlowContainer, Context context)
        {
            _specFlowContainer = specFlowContainer;
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
                _specFlowContainer.RegisterInstanceAs(_context.Container.GetInstance(action));
        }
    }
}
