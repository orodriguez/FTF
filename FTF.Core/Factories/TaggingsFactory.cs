using FTF.Core.Delegates;
using FTF.Core.Entities;

namespace FTF.Core.Factories
{
    public class TaggingsFactory
    {
        private readonly GetCurrentTime _getCurrentTime;

        public TaggingsFactory(GetCurrentTime getCurrentTime)
        {
            _getCurrentTime = getCurrentTime;
        }

        public Tagging Make(Note note, Tag tag) => new Tagging
        {
            Note = note,
            Tag = tag,
            CreationDate = _getCurrentTime(),
        };
    }
}