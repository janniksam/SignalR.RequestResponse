using System.IO;
using System.Xml.Serialization;

namespace SignalR.Request.Response.Shared
{
    public static class Serializer
    {
        public static string SerializeObject<T>(this T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        public static T DeserializeObject<T>(string toDeserialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (StringReader reader = new StringReader(toDeserialize))
            {
                T obj = (T)xmlSerializer.Deserialize(reader);
                return obj;
            }
        }
    }
}