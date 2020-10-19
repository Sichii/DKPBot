using System.IO;
using System.Xml.Serialization;

namespace DKPBot.Definitions
{
    internal static class XmlConvert
    {
        internal static T DeserializeObject<T>(Stream stream)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            var obj = xmlSerializer.Deserialize(stream);
            return (T) obj;
        }

        internal static void SerializeObject<T>(T obj, Stream stream)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            xmlSerializer.Serialize(stream, obj);
        }
    }
}