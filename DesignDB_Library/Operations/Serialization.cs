using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace DesignDB_Library.Operations
{
    public class Serialization
    {
        public static string SerializeToXml<T>(T value)
        {
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(writer, value);
            return writer.ToString();
        }

        public static T DeserializeToList<T>(string xml)
        {  
            // Create an instance of the XmlSerializer.
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            // Declare an object variable of the type to be deserialized.
            //T output;

            StringReader reader = new StringReader(xml);
            //var list = (List<T>)serializer.Deserialize(reader);
            T list = (T)serializer.Deserialize(reader);
            return list;
        }
    }
}