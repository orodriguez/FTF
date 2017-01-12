using System;

namespace FTF.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RoleAttribute : Attribute
    {
        public Type RoleType { get; }

        public RoleAttribute(Type roleType)
        {
            RoleType = roleType;
        }
    }
}