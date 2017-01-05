using System;

namespace FTF.Core
{
    public class RecordNotFoundException : ApplicationException
    {
        public RecordNotFoundException(int id, string entityName) : base($"{entityName} with id #{id} does not exist")
        {
        }
    }
}