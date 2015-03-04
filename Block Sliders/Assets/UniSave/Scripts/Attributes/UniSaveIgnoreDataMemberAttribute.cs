using System;

namespace UniSave.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class UniSaveIgnoreDataMemberAttribute : Attribute
    {
    }
}
