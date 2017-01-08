using System;
using FTF.Core.Attributes;
using FTF.Core.Entities;
using Action = System.Action;

namespace FTF.Core.Tags
{
    [Concrete]
    public class CreateTagHandler
    {
        private readonly Action<Tag> _save;

        private readonly Action _saveChanges;

        public CreateTagHandler(Action<Tag> save, Action saveChanges)
        {
            _save = save;
            _saveChanges = saveChanges;
        }

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