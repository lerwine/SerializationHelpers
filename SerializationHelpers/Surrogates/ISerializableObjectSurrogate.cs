using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace SerializationHelpers.Surrogates
{
    public interface ISerializableObjectSurrogate : IObjectSurrogate, ISerializable, IXmlSerializable
    {
    }

    public interface ISerializableObjectSurrogate<T> : ISerializableObjectSurrogate, IObjectSurrogate<T>
    {
    }
}
