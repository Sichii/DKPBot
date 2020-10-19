using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "player")]
    public class Player
    {
        [XmlElement(ElementName = "id", DataType = "int")]
        public int Id { get; set; }

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "active")]
        public string Active { get; set; }

        [XmlElement(ElementName = "hidden")]
        public string Hidden { get; set; }

        [XmlElement(ElementName = "main_id")]
        public string MainId { get; set; }

        [XmlElement(ElementName = "main_name")]
        public string MainName { get; set; }

        [XmlElement(ElementName = "class_id")]
        public string ClassId { get; set; }

        [XmlElement(ElementName = "class_name")]
        public string ClassName { get; set; }

        [XmlElement(ElementName = "points")]
        public Points Points { get; set; }

        [XmlElement(ElementName = "items")]
        public string Items { get; set; }

        [XmlElement(ElementName = "adjustments")]
        public string Adjustments { get; set; }
    }
}