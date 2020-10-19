using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "events")]
    public class Events
    {
        [XmlElement(ElementName = "event")]
        public Event Event { get; set; }
    }
}