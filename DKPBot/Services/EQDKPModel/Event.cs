using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "event")]
    public class Event
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "value")]
        public string Value { get; set; }
    }
}