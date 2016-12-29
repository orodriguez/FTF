using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public ICollection<Tag> Tags { get; set; }

        public Note() : this(new Collection<Tag>())
        {
        }

        private Note(ICollection<Tag> tags)
        {
            Tags = tags;
        }
    }
}