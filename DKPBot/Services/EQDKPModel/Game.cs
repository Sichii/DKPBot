using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "game")]
    public class Game
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "version")]
        public string Version { get; set; }

        [XmlElement(ElementName = "language")]
        public string Language { get; set; }

        [XmlElement(ElementName = "server_name")]
        public string ServerName { get; set; }

        [XmlElement(ElementName = "server_loc")]
        public string ServerLoc { get; set; }
    }
}