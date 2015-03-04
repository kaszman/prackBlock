using System;
using ProtoBuf;

namespace UniSave.Containers
{
    [ProtoContract]
    public sealed class MemberContainer : Container
    {
        [ProtoMember(1)] public MemberType Type { get; private set; }
        [ProtoMember(2)] public int ObjectTypeIndex { get; private set; }
        [ProtoMember(3)] public int NameIndex { get; private set; }
        [ProtoMember(4)] public int ValueIndex { get; private set; }
        [ProtoMember(5)] public bool IsNull { get; private set; }

        public static MemberContainer CreateContainer(MemberType type, Type objectType, string name, string value, bool isNull = false)
        {
            var container = new MemberContainer
            {
                Type = type,
                ObjectTypeIndex = OptimizationContainer.AddType(objectType),
                NameIndex = OptimizationContainer.AddMember(name),
                ValueIndex = OptimizationContainer.AddValue(value),
                IsNull = isNull
            };

            return container;
        }

        // For ProtoBuf deserialization.
        private MemberContainer()
        {
        }
    }
}