using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FTF.Api.Notes.Retrieve;

namespace FTF.Core
{
    public class Note : IResponse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreationDate { get; set; }

        public string UserName => User.Name;

        public virtual User User { get; set; }

        public ICollection<ITag> Tags { get; set; }

        public Note() : this(new Collection<ITag>())
        {
        }

        private Note(ICollection<ITag> tags)
        {
            Tags = tags;
        }
    }
}