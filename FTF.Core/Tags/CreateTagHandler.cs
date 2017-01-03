using System;
using FTF.Core.Entities;

namespace FTF.Core.Tags
{
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