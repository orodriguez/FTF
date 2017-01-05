namespace FTF.Core.Entities
{
    public class Tagging : IEntity
    {
        public int Id { get; set; }
        public Note Note { get; set; }
        public Tag Tag { get; set; }
    }
}