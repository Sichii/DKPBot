using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "info")]
    public class Info
    {
        [XmlElement(ElementName = "with_twink")]
        public string WithTwink { get; set; }

        [XmlElement(ElementName = "date")]
        public string Date { get; set; }

        [XmlElement(ElementName = "timestamp")]
        public string Timestamp { get; set; }

        [XmlElement(ElementName = "total_players")]
        public string TotalPlayers { get; set; }

        [XmlElement(ElementName = "total_items")]
        public string TotalItems { get; set; }
    }
}