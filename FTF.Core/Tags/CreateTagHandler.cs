using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.Delegates.Actions.Tags;
using FTF.Core.Entities;

namespace FTF.Core.Tags
{
    [Concrete]
    public class CreateTagHandler
    {
        private readonly Save<Tag> _save;

        private readonly SaveChanges _saveChanges;

        public CreateTagHandler(Save<Tag> save, SaveChanges saveChanges)
        {
            _save = save;
            _saveChanges = saveChanges;
        }

        [Role(typeof(Create))]
        public void Create(string name)
        {
            _save(new Tag
            {
                Name = name
            });

            _saveChanges();
        }
    }
}