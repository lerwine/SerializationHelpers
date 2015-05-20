using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SerializationHelpers
{
    public static class SerializationUtility
    {
        private static Dictionary<string, Dictionary<Type, DataContractSerializer>> _dataContractSerializerCache = new Dictionary<string, Dictionary<Type, DataContractSerializer>>();

        public static void SerializeObject(XmlWriter xmlWriter, object obj, string elementName = null)
        {
            string rootName = (elementName == null) ? "" : elementName.Trim();
            Type type = (obj == null) ? typeof(object) : obj.GetType();
            DataContractSerializer serializer;
            lock (SerializationUtility._dataContractSerializerCache)
            {
                Dictionary<Type, DataContractSerializer> dict;
                if (SerializationUtility._dataContractSerializerCache.ContainsKey(rootName))
                    dict = SerializationUtility._dataContractSerializerCache[rootName];
                else
                {
                    dict = new Dictionary<Type, DataContractSerializer>();
                    SerializationUtility._dataContractSerializerCache.Add(rootName, dict);
                }

                if (dict.ContainsKey(type))
                    serializer = dict[type];
                else
                {
                    if (String.IsNullOrWhiteSpace(elementName))
                        serializer = new DataContractSerializer(type, new DataContractSerializerSettings { DataContractSurrogate = new SerializationSurrogateSelector() });
                    else
                    {
                        XmlDictionary ds = new XmlDictionary();
                        serializer = new DataContractSerializer(type, new DataContractSerializerSettings { DataContractSurrogate = new SerializationSurrogateSelector(), RootName = ds.Add(rootName) });
                    }
                    if (dict.Count > 1023)
                        dict.Remove(dict.First().Key);
                    dict.Add(type, serializer);
                }
            }

            serializer.WriteObject(xmlWriter, obj);
        }

        private static TResult SerializeObjectFrom<TResult>(object obj, Func<XmlReader, TResult> func)
        {
            TResult result;

            using (MemoryStream stream = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    CloseOutput = false,
                    Encoding = Encoding.UTF8,
                    Indent = true
                };
                using (XmlWriter xmlWriter = XmlWriter.Create(stream, settings))
                {
                    SerializationUtility.SerializeObject(xmlWriter, obj);
                    xmlWriter.Flush();
                }

                stream.Seek(0L, SeekOrigin.Begin);

                using (XmlReader xmlReader = XmlReader.Create(stream))
                    result = func(xmlReader);
            }

            return result;
        }

        public static XDocument SerializeObjectToXDocument(object obj)
        {
            return SerializationUtility.SerializeObjectFrom<XDocument>(obj, (XmlReader xmlReader) => XDocument.Load(xmlReader));
        }

        public static XmlDocument SerializeObjectToXmlDocument(object obj)
        {
            return SerializationUtility.SerializeObjectFrom<XmlDocument>(obj, (XmlReader xmlReader) =>
            {
                XmlDocument result = new XmlDocument();
                result.Load(xmlReader);
                return result;
            });
        }
    }
}
