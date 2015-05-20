using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SerializationHelpers.Surrogates
{
    [Serializable]
    public class SerializableObjectSurrogate<T> : ISerializableObjectSurrogate<T>
    {
        #region IObjectSurrogate Members

        public T Deserialized_Object { get; private set; }

        object IObjectSurrogate.Deserialized_Object
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        public SerializableObjectSurrogate() : this(default(T)) { }

        public SerializableObjectSurrogate(T obj)
        {
            this.Deserialized_Object = this.OnObjectDeserialized(obj);
        }

        #region ISerializable Members

        public SerializableObjectSurrogate(SerializationInfo info, StreamingContext context)
        {
            T obj;

            try
            {
                obj = (T)(info.GetValue("Deserialized_Object", typeof(T)));
            }
            catch
            {
                obj = default(T);
            }

            BinaryFormatter formatter = new BinaryFormatter();
            this.Deserialized_Object = this.OnObjectDeserialized(obj);
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Deserialized_Object", this.Deserialized_Object);
        }

        #endregion

        protected virtual T OnObjectDeserialized(T result) { return result; }

        #region IXmlSerializable Members

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            throw new NotImplementedException();
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Attribute:
                        this.OnReadAttribute(reader.LocalName, reader.NamespaceURI, reader.Value);
                        break;
                    case XmlNodeType.Element:
                        this.OnReadElement(reader);
                        break;
                    case XmlNodeType.Text:
                        this.OnReadText(reader.Value);
                        break;
                    case XmlNodeType.SignificantWhitespace:
                        this.OnReadWhitespace(reader.Value, true);
                        break;
                    case XmlNodeType.Whitespace:
                        this.OnReadWhitespace(reader.Value, false);
                        break;
                }
            }
        }

        protected virtual bool OnReadAttribute(string localName, string namespaceURI, string value) { return false; }

        protected virtual bool OnReadElement(XmlReader reader)
        {
            if (reader == null || reader.NamespaceURI != "" || reader.LocalName != "Raw.Data")
                return false;

            T deserializedObject = default(T);

            if (!reader.IsEmptyElement)
            {
                try
                {
                    string txt = reader.GetAttribute("Length");
                    int i;
                    if (!String.IsNullOrWhiteSpace(txt) && Int32.TryParse(txt.Trim(), out i) && i > 0)
                    {
                        byte[] buffer = new byte[i];
                        reader.ReadContentAsBase64(buffer, 0, i);
                        using (MemoryStream stream = new MemoryStream(buffer))
                        {
                            BinaryFormatter formatter = new BinaryFormatter();
                            deserializedObject = (T)(formatter.Deserialize(stream));
                        }
                    }
                }
                catch { }
            }

            this.Deserialized_Object = this.OnObjectDeserialized(deserializedObject);

            return true;
        }

        protected virtual bool OnReadText(string text) { return false; }

        protected virtual bool OnReadWhitespace(string text, bool isSignificant) { return false; }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if ((object)(this.Deserialized_Object) == null)
                return;

            this.WriteAttributes(writer);
            this.WriteTopElements(writer);
            this.WriteMidElements(writer);
            this.WriteBottomElements(writer);
        }

        protected void TryWriteTextAttribute(XmlWriter writer, string name, Func<string> getValue)
        {
            string s;
            try
            {
                s = getValue();
            }
            catch
            {
                s = null;
            }

            if (s != null)
                writer.WriteAttributeString(name, s);
        }

        protected void TryWriteTextElement(XmlWriter writer, string name, Func<string> getValue)
        {
            string s;
            try
            {
                s = getValue();
            }
            catch
            {
                s = null;
            }

            if (s == null)
                return;

            writer.WriteStartElement(name);
            writer.WriteCData(s);
            writer.WriteEndElement();
        }

        protected virtual void WriteAttributes(XmlWriter writer)
        {
            writer.WriteAttributeString("Type", typeof(T).FullName);
        }

        protected virtual void WriteTopElements(XmlWriter writer) { }

        protected virtual void WriteMidElements(XmlWriter writer) { }

        protected virtual void WriteBottomElements(XmlWriter writer)
        {
            if ((object)(this.Deserialized_Object) == null)
                return;

            BinaryFormatter formatter = new BinaryFormatter();
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                formatter.Serialize(stream, this.Deserialized_Object);
                writer.WriteStartElement("Raw.Data");
                byte[] buffer = stream.ToArray();
                writer.WriteBase64(buffer, 0, buffer.Length);
                writer.WriteEndElement();
            }
        }

        #endregion

    }
}
