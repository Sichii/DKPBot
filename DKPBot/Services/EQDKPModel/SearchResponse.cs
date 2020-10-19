using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "response")]
    public class SearchResponse<T>
    {
        [XmlElement(ElementName = "direct")]
        public Direct<T> Direct { get; set; }

        [XmlElement(ElementName = "relevant")]
        public Relevant<T> Relevant { get; set; }

        [XmlElement(ElementName = "status")]
        public string Status { get; set; }
    }
}