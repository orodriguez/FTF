using System;

namespace FTF.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class Role : Attribute
    {
        public Type DelegateType { get; }

        public Role(Type delegateType)
        {
            DelegateType = delegateType;
        }
    }
}