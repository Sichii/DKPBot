using System.Collections.Generic;
using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "points")]
    public class Points
    {
        [XmlElement(ElementName = "multidkp_points")]
        public List<Multidkp_points> MultidkpPoints { get; set; }
    }
}