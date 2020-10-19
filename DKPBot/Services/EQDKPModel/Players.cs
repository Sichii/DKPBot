using System.Collections.Generic;
using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "players")]
    public class Players
    {
        [XmlElement(ElementName = "player")]
        public List<Player> Player { get; set; }
    }
}