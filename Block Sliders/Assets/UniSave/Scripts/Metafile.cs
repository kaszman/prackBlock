using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using ProtoBuf;

namespace UniSave
{
    /// <summary>
    /// Contains information (metadata) about a save file.
    /// </summary>
    [ProtoContract]
    public sealed class Metafile : IXmlSerializable
    {
        /// <summary>
        /// Gets the name of the save file.
        /// </summary>
        [ProtoMember(1)] public string Name { get; private set; }
        
        /// <summary>
        /// Gets the date when the save file was last saved.
        /// </summary>
        [ProtoMember(2)] public string Date { get; private set; }

        /// <summary>
        /// Gets the name of the scene that was last saved. Only applicable if the save file is of the Scene File type.
        /// </summary>
        [ProtoMember(3)] public string LastLevel { get; private set; }

        [ProtoMember(4)] private readonly List<Metafield> _customFields = new List<Metafield>();

        [ProtoMember(5)] private readonly FileType _fileType;

        public Metafile(string name, string date, string lastLevel, Metafield[] metafields, FileType fileType)
        {
            Name = name;
            Date = date;
            LastLevel = lastLevel;

            if (metafields != null)
            {
                foreach (var data in metafields)
                {
                    _customFields.Add(data);
                }
            }

            _fileType = fileType;
        }

        public FileType GetFileType()
        {
            return _fileType;
        }

        /// <summary>
        /// Enumerates through the list of custom metafields.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Metafield> GetMetafields()
        {
            return _customFields.AsEnumerable();
        }

        /// <summary>
        /// Finds the metafield with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The value of the metafield; or null when not found.</returns>
        public string FindMetafield(string name)
        {
            return _customFields.Find(x => x.Name == name).Value;
        }

        // For ProtoBuf serialization.
        private Metafile() {}

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.Read();

            Name = reader.ReadElementString("Name");
            Date = reader.ReadElementString("Date");
            LastLevel = reader.ReadElementString("LastLevel");

            var count = Convert.ToInt32(reader.ReadElementString("Count"));

            for (var i = 0; i < count; i++)
            {
                var name = reader.ReadElementString("Name");
                var value = reader.ReadElementString("Value");

                _customFields.Add(new Metafield(name, value));
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Name", Name);
            writer.WriteElementString("Date", Date);
            writer.WriteElementString("LastLevel", LastLevel);

            writer.WriteElementString("Count", _customFields.Count.ToString());

            foreach (var field in _customFields)
            {
                writer.WriteElementString("Name", field.Name);
                writer.WriteElementString("Value", field.Value);
            }
        }
    }
}
