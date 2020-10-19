using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "direct", IsNullable = true)]
    public class Direct<T>
    {
        [XmlElement(ElementName = "member")]
        public T Result { get; set; }
    }
}