using System.Xml.Serialization;

namespace DKPBot.Services.EQDKPModel
{
    [XmlRoot(ElementName = "multidkp_points")]
    public class Multidkp_points
    {
        [XmlElement(ElementName = "multidkp_id", DataType = "int")]
        public int MultidkpId { get; set; }

        [XmlElement(ElementName = "points_current", DataType = "int")]
        public int PointsCurrent { get; set; }

        [XmlElement(ElementName = "points_current_with_twink")]
        public string PointsCurrentWithTwink { get; set; }

        [XmlElement(ElementName = "points_earned")]
        public string PointsEarned { get; set; }

        [XmlElement(ElementName = "points_earned_with_twink")]
        public string PointsEarnedWithTwink { get; set; }

        [XmlElement(ElementName = "points_spent")]
        public string PointsSpent { get; set; }

        [XmlElement(ElementName = "points_spent_with_twink")]
        public string PointsSpentWithTwink { get; set; }

        [XmlElement(ElementName = "points_adjustment")]
        public string PointsAdjustment { get; set; }

        [XmlElement(ElementName = "points_adjustment_with_twink")]
        public string PointsAdjustmentWithTwink { get; set; }
    }
}