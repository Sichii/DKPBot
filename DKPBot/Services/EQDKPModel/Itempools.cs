using System.Collections.Generic;
using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "itempools")]
    public class Itempools
    {
        [XmlElement(ElementName = "itempool")]
        public List<Itempool> Itempool { get; set; }
    }
}