using System;
using System.Collections.Generic;
using FTF.Api.Actions.Notes;
using FTF.Api.Responses;
using FTF.Core.Entities;

namespace FTF.Core.Notes.Retrieve
{
    internal class Response : INote
    {
        private readonly Note _note;

        public Response(Note note)
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