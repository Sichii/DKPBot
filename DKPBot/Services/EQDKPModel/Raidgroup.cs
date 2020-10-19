using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "raidgroup")]
    public class Raidgroup
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "default")]
        public string Default { get; set; }

        [XmlElement(ElementName = "color")]
        public string Color { get; set; }

        [XmlElement(ElementName = "status")]
        public string Status { get; set; }
    }
}