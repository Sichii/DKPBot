using System.Collections.Generic;
using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "relevant", IsNullable = true)]
    public class Relevant<T>
    {
        [XmlElement(ElementName = "member")]
        public List<T> Results { get; set; }
    }
}