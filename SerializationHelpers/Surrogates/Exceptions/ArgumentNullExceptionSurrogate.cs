using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SerializationHelpers.Surrogates.Exceptions
{
    public class ArgumentNullExceptionSurrogate<TException> : ArgumentExceptionSurrogate<TException>
        where TException : ArgumentNullException
    {
        public ArgumentNullExceptionSurrogate() : base() { }

        public ArgumentNullExceptionSurrogate(TException exception) : base(exception) { }

        public ArgumentNullExceptionSurrogate(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
