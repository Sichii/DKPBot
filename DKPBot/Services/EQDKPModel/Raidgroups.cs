using System.Collections.Generic;
using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "raidgroups")]
    public class Raidgroups
    {
        [XmlElement(ElementName = "raidgroup")]
        public List<Raidgroup> Raidgroup { get; set; }
    }
}