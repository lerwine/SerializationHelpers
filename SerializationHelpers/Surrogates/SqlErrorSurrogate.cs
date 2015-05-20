using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SerializationHelpers.Surrogates
{
    public class SqlErrorSurrogate : SerializableObjectSurrogate<SqlError>
    {
        public SqlErrorSurrogate() : base() { }

        public SqlErrorSurrogate(SqlError error) : base(error) { }

        public SqlErrorSurrogate(SerializationInfo info, StreamingContext context) : base(info, context) { }

        protected override void WriteAttributes(XmlWriter writer)
        {
            if (this.Deserialized_Object != null)
            {
                this.TryWriteTextAttribute(writer, "Number", () => this.Deserialized_Object.Number.ToString());
            }

            base.WriteAttributes(writer);
        }

        protected override void WriteTopElements(XmlWriter writer)
        {
            if (this.Deserialized_Object != null)
            {
                string txt;
                try
                {
                    txt = this.Deserialized_Object.Message;
                }
                catch
                {
                    txt = null;
                }
                writer.WriteStartElement("Message");
                this.TryWriteTextAttribute(writer, "Class", () => ((int)(this.Deserialized_Object.Class)).ToString());
                this.TryWriteTextAttribute(writer, "State", () => ((int)(this.Deserialized_Object.State)).ToString());
                if (txt != null)
                    writer.WriteCData(txt);
                writer.WriteEndElement();
            }

            base.WriteTopElements(writer);
        }

        protected override void WriteMidElements(XmlWriter writer)
        {
            base.WriteMidElements(writer);

            if (this.Deserialized_Object == null)
                return;

            string txt;
            try
            {
                txt = this.Deserialized_Object.Procedure;
            }
            catch
            {
                txt = null;
            }
            writer.WriteStartElement("Procedure");
            this.TryWriteTextAttribute(writer, "LineNumber", () => this.Deserialized_Object.LineNumber.ToString());
            this.TryWriteTextAttribute(writer, "Server", () => this.Deserialized_Object.Server);
            if (txt != null)
                writer.WriteCData(txt);
            writer.WriteEndElement();

            this.TryWriteTextElement(writer, "Source", () => this.Deserialized_Object.Source);
        }
    }
}
