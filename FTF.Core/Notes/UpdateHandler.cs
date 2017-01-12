using System.Linq;
using FTF.Core.Attributes;
using FTF.Core.Delegates;
using FTF.Core.Entities;
using FTF.Core.Storage;

namespace FTF.Core.Notes
{
    [Concrete]
    public class UpdateHandler
    {
        private readonly IQueryable<Note> _notes;

        private readonly IUnitOfWork _uow;

        private readonly ValidateNote _validate;

        public UpdateHandler(
            IQueryable<Note> notes, 
            ValidateNote validate, 
            IUnitOfWork uow)
        {
            _notes = notes;
            _validate = validate;
            _uow = uow;
        }

        public void Update(int id, string text)
        {
            _validate(text);

            var noteToUpdate = _notes.First(n => n.Id == id);

            noteToUpdate.Text = text;

            _uow.SaveChanges();
        }
    }
}