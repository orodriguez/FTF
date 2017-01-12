using FTF.Api.Exceptions;

namespace FTF.Core.Notes
{
    public class NoteValidator
    {
        public static void Validate(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ValidationException("Note can not be empty");
        }
    }
}