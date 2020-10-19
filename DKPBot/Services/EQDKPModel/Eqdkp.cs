using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "eqdkp")]
    public class Eqdkp
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "guild")]
        public string Guild { get; set; }

        [XmlElement(ElementName = "dkp_name")]
        public string DkpName { get; set; }

        [XmlElement(ElementName = "version")]
        public string Version { get; set; }

        [XmlElement(ElementName = "layout")]
        public string Layout { get; set; }

        [XmlElement(ElementName = "base_layout")]
        public string BaseLayout { get; set; }
    }
}