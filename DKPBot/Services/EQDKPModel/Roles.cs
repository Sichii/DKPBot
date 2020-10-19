using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "roles")]
    public class Roles
    {
        [XmlElement(ElementName = "role")]
        public Role Role { get; set; }
    }
}