using System;
using System.Linq.Expressions;

namespace FTF.Core.Entities
{
    public class Tagging : IEntity
    {
        public int Id { get; set; }
        public Note Note { get; set; }
        public Tag Tag { get; set; }
        public DateTime CreationDate { get; set; }

        public static Expression<Func<Tagging, bool>> TagCreatedByUser(int userId) => tn => tn.Tag.User.Id == userId;

        public static Expression<Func<Tagging, bool>> Trash => t => t.Tag.Name == "Trash";
    }
}