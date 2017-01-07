using System;

namespace FTF.Core.Attributes
{
    public class Action : Attribute
    {
        public Type DelegateType { get; }

        public Action(Type delegateType)
        {
            DelegateType = delegateType;
        }
    }
}