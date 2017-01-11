using System;

namespace FTF.Api.Exceptions
{
    public class RecordNotFoundException : ApplicationException
    {
        public RecordNotFoundException(int id, string entityName) 
            : base($"{entityName} with id #{id} does not exist")
        {
        }
    }
}