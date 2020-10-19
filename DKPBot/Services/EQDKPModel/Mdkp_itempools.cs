using System.Collections.Generic;
using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "mdkp_itempools")]
    public class Mdkp_itempools
    {
        [XmlElement(ElementName = "itempool_id")]
        public List<string> ItempoolId { get; set; }
    }
}