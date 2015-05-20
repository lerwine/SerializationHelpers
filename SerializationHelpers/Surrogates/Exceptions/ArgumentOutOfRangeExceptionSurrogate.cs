using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SerializationHelpers.Surrogates.Exceptions
{
    public class ArgumentOutOfRangeExceptionSurrogate<TException> : ArgumentExceptionSurrogate<TException>
        where TException : ArgumentOutOfRangeException
    {
        public ArgumentOutOfRangeExceptionSurrogate() : base() { }

        public ArgumentOutOfRangeExceptionSurrogate(TException exception) : base(exception) { }

        public ArgumentOutOfRangeExceptionSurrogate(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
