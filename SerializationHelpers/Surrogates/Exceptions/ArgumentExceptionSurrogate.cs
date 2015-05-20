using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SerializationHelpers.Surrogates.Exceptions
{
    public class ArgumentExceptionSurrogate<TException> : ExceptionSurrogate<TException>
        where TException : ArgumentException
    {
        public ArgumentExceptionSurrogate() : base() { }

        public ArgumentExceptionSurrogate(TException exception) : base(exception) { }

        public ArgumentExceptionSurrogate(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
