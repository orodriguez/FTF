using System;

namespace FTF.Core
{
    public class ValidationException : ApplicationException
    {
        public ValidationException(string message) : base(message)
        {
        }
    }
}