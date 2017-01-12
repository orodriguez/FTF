using System;

namespace FTF.Core.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class RoleAttribute : Attribute
    {
        public Type RoleType { get; }

        public RoleAttribute(Type roleType)
        {
            RoleType = roleType;
        }
    }
}