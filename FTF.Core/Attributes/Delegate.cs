using System;

namespace FTF.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class Delegate : Attribute
    {
        public Type DelegateType { get; }

        public Delegate(Type delegateType)
        {
            DelegateType = delegateType;
        }
    }
}