using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "multidkp_pool")]
    public class Multidkp_pool
    {
        [XmlElement(ElementName = "id", DataType = "int")]
        public int Id { get; set; }

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "desc")]
        public string Desc { get; set; }

        [XmlElement(ElementName = "events")]
        public Events Events { get; set; }

        [XmlElement(ElementName = "mdkp_itempools")]
        public Mdkp_itempools MdkpItempools { get; set; }
    }
}