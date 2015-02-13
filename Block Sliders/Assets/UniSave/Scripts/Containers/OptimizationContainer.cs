using System;
using System.Collections.Generic;
using ProtoBuf;

namespace UniSave.Containers
{
    [ProtoContract]
    public sealed class OptimizationContainer
    {
        [ProtoMember(1)] public List<string> Types = new List<string>();
        [ProtoMember(2)] public List<string> Members = new List<string>(); 
        [ProtoMember(3)] public List<string> ObjectNames = new List<string>();
        [ProtoMember(4)] public List<string> Values = new List<string>();

        public static OptimizationContainer _instance;

        // For ProtoBuf deserialization.
        public OptimizationContainer()
        {
        }

        public static int AddType(Type type)
        {
            if (_instance.Types.Exists(x => x == type.AssemblyQualifiedName))
            {
                return _instance.Types.FindIndex(x => x == type.AssemblyQualifiedName);
            }

            _instance.Types.Add(type.AssemblyQualifiedName);
            return _instance.Types.Count - 1;
        }

        public static string GetTypeName(int index)
        {
            return _instance.Types[index];
        }

        public static Type GetType(MemberContainer container)
        {
            return Type.GetType(_instance.Types[container.ObjectTypeIndex]);
        }

        public static bool TypesAreIdentical(Type type, int serializedTypeIndex)
        {
            return type.AssemblyQualifiedName == _instance.Types[serializedTypeIndex];
        }

        public static int AddMember(string name)
        {
            if (_instance.Members.Exists(x => x == name))
            {
                return _instance.Members.FindIndex(x => x == name);
            }

            _instance.Members.Add(name);
            return _instance.Members.Count - 1;
        }

        public static string GetMemberName(int index)
        {
            return _instance.Members[index];
        }

        public static int AddObjectName(string name)
        {
            if (_instance.ObjectNames.Exists(x => x == name))
            {
                return _instance.ObjectNames.FindIndex(x => x == name);
            }

            _instance.ObjectNames.Add(name);
            return _instance.ObjectNames.Count - 1;
        }

        public static string GetObjectName(int index)
        {
            return _instance.ObjectNames[index];
        }

        public static int AddValue(string value)
        {
            if (_instance.Values.Exists(x => x == value))
            {
                return _instance.Values.FindIndex(x => x == value);
            }

            _instance.Values.Add(value);
            return _instance.Values.Count - 1;
        }

        public static string GetValue(MemberContainer container)
        {
            return _instance.Values[container.ValueIndex];
        }
    }
}
