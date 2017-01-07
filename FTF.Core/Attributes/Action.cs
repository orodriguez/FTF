using System;

namespace FTF.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class Action : Attribute
    {
        public Type DelegateType { get; }

        public Action(Type delegateType)
        {
            DelegateType = delegateType;
        }
    }
}