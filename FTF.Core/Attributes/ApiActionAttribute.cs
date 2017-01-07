using System;

namespace FTF.Core.Attributes
{
    public class ApiActionAttribute : Attribute
    {
        public Type DelegateType { get; }

        public ApiActionAttribute(Type delegateType)
        {
            DelegateType = delegateType;
        }
    }
}