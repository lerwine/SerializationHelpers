using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace SerializationHelpers.Surrogates.Exceptions
{
    public class ExternalExceptionSurrogate<TException> : ExceptionSurrogate<TException>
        where TException : ExternalException
    {
        public ExternalExceptionSurrogate() : base() { }

        public ExternalExceptionSurrogate(TException exception) : base(exception) { }

        public ExternalExceptionSurrogate(SerializationInfo info, StreamingContext context) : base(info, context) { }

        protected override void WriteAttributes(XmlWriter writer)
        {
            if (this.Deserialized_Object != null)
                this.TryWriteTextAttribute(writer, "ErrorCode.", () => this.Deserialized_Object.ErrorCode.ToString());

            base.WriteAttributes(writer);
        }
    }
}
