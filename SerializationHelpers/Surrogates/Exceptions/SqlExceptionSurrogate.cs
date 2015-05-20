using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SerializationHelpers.Surrogates.Exceptions
{
    public class SqlExceptionSurrogate : ExternalExceptionSurrogate<SqlException>
    {
        public SqlExceptionSurrogate() : base() { }

        public SqlExceptionSurrogate(SqlException exception) : base(exception) { }

        public SqlExceptionSurrogate(SerializationInfo info, StreamingContext context) : base(info, context) { }

        protected override void WriteAttributes(XmlWriter writer)
        {
            if (this.Deserialized_Object != null)
            {
                this.TryWriteTextAttribute(writer, "Number", () => this.Deserialized_Object.Number.ToString());
                this.TryWriteTextAttribute(writer, "Class", () => ((int)(this.Deserialized_Object.Class)).ToString());
                this.TryWriteTextAttribute(writer, "State", () => ((int)(this.Deserialized_Object.State)).ToString());
            }

            base.WriteAttributes(writer);
        }

        protected override void WriteMidElements(XmlWriter writer)
        {
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

            base.WriteMidElements(writer);
        }

        protected override void WriteBottomElements(XmlWriter writer)
        {
            if (this.Deserialized_Object != null && this.Deserialized_Object.Errors != null)
                SerializationUtility.SerializeObject(writer, this.Deserialized_Object.Errors);

            base.WriteBottomElements(writer);
        }
    }
}
