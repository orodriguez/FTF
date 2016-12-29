using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FTF.Core.Notes.Retrieve;

namespace FTF.Core
{
    public class Note : IResponse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreationDate { get; set; }

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