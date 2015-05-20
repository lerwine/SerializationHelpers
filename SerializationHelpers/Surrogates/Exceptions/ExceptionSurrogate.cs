using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SerializationHelpers.Surrogates.Exceptions
{
    public class ExceptionSurrogate<TException> : SerializableObjectSurrogate<TException>
        where TException : Exception
    {
        public ExceptionSurrogate() : base() { }

        public ExceptionSurrogate(TException exception) : base(exception) { }

        public ExceptionSurrogate(SerializationInfo info, StreamingContext context) : base(info, context) { }

        protected override void WriteAttributes(XmlWriter writer)
        {
            if (this.Deserialized_Object != null)
            {
                writer.WriteAttributeString("HResult", this.Deserialized_Object.HResult.ToString());
                if (this.Deserialized_Object.TargetSite != null)
                    writer.WriteAttributeString("TargetSite", this.Deserialized_Object.TargetSite.ToString());
            }

            base.WriteAttributes(writer);
        }

        protected override void WriteTopElements(XmlWriter writer)
        {
            if (this.Deserialized_Object != null)
                this.TryWriteTextElement(writer, "Message", () => this.Deserialized_Object.Message);

            base.WriteTopElements(writer);
        }

        protected override void WriteMidElements(XmlWriter writer)
        {
            if (this.Deserialized_Object != null)
            {
                this.TryWriteTextElement(writer, "HelpLink", () => this.Deserialized_Object.HelpLink);
                this.TryWriteTextElement(writer, "Source", () => this.Deserialized_Object.Source);
            }

            base.WriteMidElements(writer);
        }

        protected override void WriteBottomElements(XmlWriter writer)
        {
            base.WriteBottomElements(writer);

            if (this.Deserialized_Object == null)
                return;

            this.TryWriteTextElement(writer, "StackTrace", () => this.Deserialized_Object.StackTrace);

            if (this.Deserialized_Object.Data != null)
                SerializationUtility.SerializeObject(writer, this.Deserialized_Object.Data, "Data");

            Exception[] innerExceptions = (this.Deserialized_Object.InnerException == null) ? new Exception[0] : new Exception[] { this.Deserialized_Object.InnerException };
            if (this.Deserialized_Object is AggregateException)
            {
                AggregateException aggregateException = this.Deserialized_Object as AggregateException;
                if (aggregateException.InnerExceptions != null)
                    innerExceptions = innerExceptions.Concat(aggregateException.InnerExceptions.Where(i => i != null && !innerExceptions.Any(e => Object.ReferenceEquals(e, i)))).ToArray();
            }

            foreach (Exception exc in innerExceptions)
                SerializationUtility.SerializeObject(writer, exc, "InnerException");
        }
    }
}
