using System.Globalization;
using ProtoBuf;

namespace UniSave
{
    /// <summary>
    /// TODO
    /// </summary>
    [ProtoContract]
    public struct Metafield
    {
        [ProtoMember(1)] public string Name { get; private set; }
        [ProtoMember(2)] public string Value { get; private set; }

        public Metafield(string name, int value) : this()
        {
            Name = name;
            Value = value.ToString(CultureInfo.InvariantCulture);
        }

        public Metafield(string name, float value): this()
        {
            Name = name;
            Value = value.ToString(CultureInfo.InvariantCulture);
        }

        public Metafield(string name, string value) : this()
        {
            Name = name;
            Value = value;
        }
    }
}
