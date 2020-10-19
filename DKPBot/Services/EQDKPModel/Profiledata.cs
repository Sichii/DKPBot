using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "profiledata")]
    public class Profiledata
    {
        [XmlElement(ElementName = "level")]
        public string Level { get; set; }

        [XmlElement(ElementName = "race")]
        public string Race { get; set; }

        [XmlElement(ElementName = "class")]
        public string Class { get; set; }

        [XmlElement(ElementName = "gender")]
        public string Gender { get; set; }

        [XmlElement(ElementName = "guild")]
        public string Guild { get; set; }

        [XmlElement(ElementName = "ascension")]
        public string Ascension { get; set; }
    }
}