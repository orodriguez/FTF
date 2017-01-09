using FTF.Core.Attributes;
using FTF.Core.Delegates;

namespace FTF.Core.Notes
{
    public class NoteValidator
    {
        [Role(typeof(ValidateNote))]
        public static void Validate(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ValidationException("Note can not be empty");
        }
    }
}