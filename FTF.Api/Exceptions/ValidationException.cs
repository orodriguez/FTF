using System;

namespace FTF.Api.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public ValidationException(string message) : base(message)
        {
        }
    }
}