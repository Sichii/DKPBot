using System.Collections.Generic;
using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "multidkp_pools")]
    public class Multidkp_pools
    {
        [XmlElement(ElementName = "multidkp_pool")]
        public List<Multidkp_pool> MultidkpPool { get; set; }
    }
}