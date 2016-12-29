using System;
using System.Collections.Generic;
using FTF.Api.Responses;

namespace FTF.Core.Responses
{
    internal class Note : INote
    {
        private readonly Entities.Note _note;

        public Note(Entities.Note note)
        {
            _note = note;
        }

        public int Id => _note.Id;
        public string Text => _note.Text;
        public DateTime CreationDate => _note.CreationDate;
        public string UserName => _note.UserName;
        public IEnumerable<ITag> Tags => _note.Tags;
    }
}