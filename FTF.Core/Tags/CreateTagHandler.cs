using FTF.Core.Attributes;
using FTF.Core.Entities;
using FTF.Core.Storage;

namespace FTF.Core.Tags
{
    [Concrete]
    public class CreateTagHandler
    {
        private readonly IRepository<Tag> _tags;

        private readonly IUnitOfWork _uow;

        public CreateTagHandler(
            IRepository<Tag> tags, 
            IUnitOfWork uow)
        {
            _tags = tags;
            _uow = uow;
        }

        public void Create(string name)
        {
            _tags.Add(new Tag
            {
                Name = name
            });

            _uow.SaveChanges();
        }
    }
}