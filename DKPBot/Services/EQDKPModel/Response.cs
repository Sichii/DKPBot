using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "response")]
    public class Response
    {
        [XmlElement(ElementName = "eqdkp")]
        public Eqdkp Eqdkp { get; set; }

        [XmlElement(ElementName = "game")]
        public Game Game { get; set; }

        [XmlElement(ElementName = "info")]
        public Info Info { get; set; }

        [XmlElement(ElementName = "players")]
        public Players Players { get; set; }

        [XmlElement(ElementName = "multidkp_pools")]
        public Multidkp_pools MultidkpPools { get; set; }

        [XmlElement(ElementName = "itempools")]
        public Itempools Itempools { get; set; }

        [XmlElement(ElementName = "status")]
        public string Status { get; set; }
    }
}