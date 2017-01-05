using System;

namespace FTF.Core.Notes
{
    public class NoteValidator
    {
        public static void Validate(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new Exception("Note can not be empty");
        }
    }
}