using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "member")]
    public class Member
    {
        [XmlElement(ElementName = "id", DataType = "int")]
        public int Id { get; set; }

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "name_export")]
        public string NameExport { get; set; }

        [XmlElement(ElementName = "main")]
        public string Main { get; set; }

        [XmlElement(ElementName = "class")]
        public string Class { get; set; }

        [XmlElement(ElementName = "classname")]
        public string Classname { get; set; }

        [XmlElement(ElementName = "roles")]
        public Roles Roles { get; set; }

        [XmlElement(ElementName = "raidgroups")]
        public Raidgroups Raidgroups { get; set; }

        [XmlElement(ElementName = "profiledata")]
        public Profiledata Profiledata { get; set; }
    }
}