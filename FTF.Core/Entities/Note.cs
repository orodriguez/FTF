using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FTF.Core.Entities
{
    public class Note
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreationDate { get; set; }

        public string UserName => User.Name;

        public virtual User User { get; set; }

        public virtual ICollection<Tagging> Taggings { get; set; }

        public IEnumerable<Tag> Tags => Taggings.Select(tn => tn.Tag);
    }
}